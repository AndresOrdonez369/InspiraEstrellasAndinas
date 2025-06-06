using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using UnityEngine.Events; // No parece que se use SegmentPlayEvent, se puede quitar

public class TimelineSegmentController : MonoBehaviour
{
    [Header("Timeline Settings")]
    [SerializeField] private PlayableDirector director;
    [Tooltip("Tiempos de inicio (en segundos) para cada segmento del timeline. ¡ESTO DEBE ESTAR CONFIGURADO!")]
    [SerializeField] private List<float> segmentStartTimes = new List<float>();

    [Header("Segment Playback Rules")]
    [Tooltip("Si es verdadero, cada segmento individual solo podrá ser reproducido una vez.")]
    [SerializeField] private bool playEachSegmentOnlyOnce = true;
    // La variable playTimelineOnlyOnce (ciclo completo único) es menos relevante si cada segmento es único.
    // Podríamos mantenerla si quieres que, después de que TODOS los segmentos (que se pudieron) se hayan reproducido,
    // ya no se pueda reproducir NINGUNO, incluso si no se usaron todos.
    // Por ahora, la comentaré para enfocarnos en la reproducción única por segmento.
    // [SerializeField] private bool playFullSequenceOnlyOnce = false;


    [Header("Optional: For specific UI/XR call")]
    [Tooltip("Índice del segmento a reproducir por el evento 'PlaySpecificSegmentFromInspector'.")]
    [SerializeField] private int segmentIndexToPlay = 0; // Útil para botones sin parámetros

    private int currentPlayingSegmentIndex = -1; // Qué segmento se está reproduciendo activamente
    private HashSet<int> playedSegmentIndices = new HashSet<int>(); // Rastrea los índices de los segmentos ya reproducidos
    // private bool fullSequenceCompleted = false; // Relacionado con playFullSequenceOnlyOnce

    void Awake()
    {
        if (director == null) director = GetComponent<PlayableDirector>();
        if (director == null)
        {
            Debug.LogError("TimelineSegmentController: PlayableDirector no asignado. Deshabilitando.", this);
            enabled = false;
            return;
        }
        if (segmentStartTimes.Count == 0)
        {
            Debug.LogWarning("TimelineSegmentController: 'Segment Start Times' está vacío. El control de segmentos no funcionará correctamente. Deshabilitando.", this);
            enabled = false;
            return;
        }
        // Es buena idea asegurarse de que el director no empiece solo si lo controlamos por script
        // director.playOnAwake = false;
    }

    // --- MÉTODO LLAMADO POR SIGNAL EMITTERS DESDE TIMELINE (SI UN SEGMENTO TERMINA NATURALMENTE) ---
    public void HandleSegmentCompletionSignal() // Renombrado para claridad
    {
        if (director != null && director.state == PlayState.Playing)
        {
            // Solo pausar si el segmento que termina es el que realmente estaba sonando
            // según nuestro 'currentPlayingSegmentIndex'. Esto evita que señales "fantasma"
            // de segmentos anteriores (si el timeline no se detuvo instantáneamente) pausen
            // un segmento nuevo.
            // Para esto, la señal necesitaría pasar el índice del segmento, o comparamos el tiempo.
            // Una solución más simple es que PlaySegment marque currentPlayingSegmentIndex,
            // y aquí solo pausamos. Si el usuario llama a otro PlaySegment, ese se encargará.

            director.Pause();
            Debug.Log($"TimelineSegmentController: Pausado por señal en {director.time}s. Segmento que estaba sonando: {currentPlayingSegmentIndex}.", this);

            // Si el segmento que terminó estaba siendo rastreado y playEachSegmentOnlyOnce es true,
            // ya fue añadido a playedSegmentIndices al iniciar en PlaySegment().
            // Si teníamos la lógica de playFullSequenceOnlyOnce, aquí se chequearía.
        }
    }

    // --- MÉTODO PRINCIPAL PARA REPRODUCIR UN SEGMENTO ---
    public void PlaySegment(int segmentIndex)
    {
        if (director == null)
        {
            Debug.LogError("TimelineSegmentController: Director es nulo.", this);
            return;
        }

        // 1. Validar Índice del Segmento
        if (segmentIndex < 0 || segmentIndex >= segmentStartTimes.Count)
        {
            Debug.LogError($"TimelineSegmentController: Índice de segmento inválido: {segmentIndex}. Segmentos definidos: {segmentStartTimes.Count}.", this);
            return;
        }

        // 2. Chequeo de "Reproducción Única por Segmento"
        if (playEachSegmentOnlyOnce && playedSegmentIndices.Contains(segmentIndex))
        {
            Debug.Log($"TimelineSegmentController: El segmento {segmentIndex} ya ha sido reproducido y 'playEachSegmentOnlyOnce' está activado. No se reproducirá de nuevo.", this);
            return;
        }

        // (Opcional: Lógica para playFullSequenceOnlyOnce si se reactiva)
        // if (playFullSequenceOnlyOnce && fullSequenceCompleted) {
        //     Debug.Log("TimelineSegmentController: La secuencia completa ya terminó y no se puede iniciar un nuevo segmento.");
        //     return;
        // }

        Debug.Log($"TimelineSegmentController: Solicitud para reproducir segmento {segmentIndex}. Segmento sonando actualmente (si alguno): {currentPlayingSegmentIndex}", this);

        // 3. Detener el Director si ya está reproduciendo algo (para evitar superposición)
        //    Esto es CRUCIAL para que no suenen a la vez.
        if (director.state == PlayState.Playing)
        {
            director.Pause(); // Pausar antes de cambiar el tiempo es más seguro que Stop() si solo queremos cambiar de clip.
                              // director.Stop() reinicia más cosas del PlayableGraph.
            Debug.Log($"TimelineSegmentController: Director pausado (estaba en {director.time}s) antes de cambiar al segmento {segmentIndex}.", this);
        }

        // 4. Actualizar estado y reproducir
        currentPlayingSegmentIndex = segmentIndex;
        director.time = segmentStartTimes[currentPlayingSegmentIndex];
        director.Play();
        Debug.Log($"TimelineSegmentController: Reproduciendo segmento {currentPlayingSegmentIndex} desde {director.time}s.", this);

        // 5. Marcar como reproducido (si aplica la regla)
        if (playEachSegmentOnlyOnce)
        {
            playedSegmentIndices.Add(currentPlayingSegmentIndex);
            Debug.Log($"TimelineSegmentController: Segmento {currentPlayingSegmentIndex} añadido a la lista de reproducidos.", this);
        }

        // (Opcional: Lógica para playFullSequenceOnlyOnce si se reactiva)
        // if (playFullSequenceOnlyOnce && currentPlayingSegmentIndex == segmentStartTimes.Count - 1) {
        //     fullSequenceCompleted = true;
        //     Debug.Log("TimelineSegmentController: Iniciando el último segmento de la secuencia única. Secuencia considerada completa.");
        // }
    }

    // --- OTROS MÉTODOS PÚBLICOS (PUEDEN NECESITAR AJUSTES) ---

    /// <summary>
    /// Intenta reproducir el siguiente segmento en orden numérico.
    /// Respetará la regla de 'playEachSegmentOnlyOnce'.
    /// </summary>
    public void PlayNextAvailableSegment() // Renombrado para más claridad
    {
        if (director == null) return;

        // (Opcional: chequeo de fullSequenceCompleted si se usa esa lógica)

        // Encontrar el siguiente segmento NO REPRODUCIDO
        int searchIndex = (currentPlayingSegmentIndex == -1) ? 0 : currentPlayingSegmentIndex + 1;

        for (int i = searchIndex; i < segmentStartTimes.Count; i++)
        {
            if (playEachSegmentOnlyOnce && playedSegmentIndices.Contains(i))
            {
                continue; // Este ya se reprodujo, saltar al siguiente
            }
            // Encontrado un segmento válido para reproducir
            Debug.Log($"TimelineSegmentController: PlayNextAvailableSegment. Intentando {i}.", this);
            PlaySegment(i);
            return;
        }

        Debug.Log("TimelineSegmentController: PlayNextAvailableSegment - No hay más segmentos disponibles o no reproducidos.", this);
        // Aquí podrías manejar el fin de la secuencia si todos los disponibles ya se usaron.
    }

    /// <summary>
    /// Para ser llamado desde el Inspector (ej. un botón sin parámetros).
    /// </summary>
    public void PlaySpecificSegmentFromInspector()
    {
        PlaySegment(segmentIndexToPlay);
    }

    /// <summary>
    /// Detiene el timeline, lo rebobina y limpia el estado de los segmentos reproducidos.
    /// </summary>
    public void ResetTimelineAndPlayedSegments()
    {
        if (director == null) return;

        director.Stop(); // Stop reinicia el director y pone el tiempo a initialTime (usualmente 0)

        currentPlayingSegmentIndex = -1;
        playedSegmentIndices.Clear();
        // fullSequenceCompleted = false; // Descomenta si reactivas esta lógica
        Debug.Log("TimelineSegmentController: Timeline y estado de segmentos reproducidos reiniciados.", this);
    }

    /// <summary>
    /// Simplemente reanuda el timeline si está pausado.
    /// No inicia un nuevo segmento ni cambia el estado de 'playedSegmentIndices'.
    /// </summary>
    public void ResumeIfPaused() // Renombrado para claridad
    {
        if (director == null) return;

        // (Opcional: chequeo de fullSequenceCompleted si se usa esa lógica y no se debe resumir)

        if (director.state == PlayState.Paused)
        {
            // Solo resumir si hay un segmento que se considera "activo"
            if (currentPlayingSegmentIndex != -1)
            {
                director.Resume();
                Debug.Log($"TimelineSegmentController: Timeline resumido. Continuará el segmento {currentPlayingSegmentIndex}.", this);
            }
            else
            {
                Debug.Log("TimelineSegmentController: Se intentó resumir, pero ningún segmento estaba activo. Considera usar PlaySegment(0) para iniciar.", this);
            }
        }
    }
}