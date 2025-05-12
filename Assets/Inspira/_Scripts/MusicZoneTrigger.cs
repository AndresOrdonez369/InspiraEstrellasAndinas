using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class MusicZoneTrigger : MonoBehaviour
{
    [Header("Audio Setup")]
    [Tooltip("El AudioSource que reproduce la m�sica de fondo principal.")]
    [SerializeField] private AudioSource musicAudioSource;

    [Tooltip("El clip de m�sica que debe sonar DENTRO de esta zona.")]
    [SerializeField] private AudioClip areaSpecificMusic;

    // NUEVO: Volumen espec�fico para la m�sica de esta �rea
    [Tooltip("El volumen al que debe sonar la m�sica espec�fica del �rea (0 a 1).")]
    [Range(0f, 1f)] // A�ade un slider en el Inspector
    [SerializeField] private float areaSpecificVolume = 0.8f; // Valor por defecto

    // --- Opcional: Para volver a la m�sica anterior ---
    [Header("Optional: Return Music")]
    [Tooltip("Si quieres volver a una m�sica espec�fica al salir (dejar vac�o para no cambiar al salir).")]
    [SerializeField] private AudioClip musicToReturnTo;
    private AudioClip originalMusicBeforeEnter;
    private float originalVolumeBeforeEnter; // NUEVO: Almacena el volumen original

    // --- Opcional: Transici�n Suave (Fade) ---
    [Header("Optional: Fading")]
    [Tooltip("Duraci�n del fundido de salida/entrada en segundos.")]
    [SerializeField] private float fadeDuration = 1.0f;
    private Coroutine fadeCoroutine = null;

    [Header("Trigger Setup")]
    [Tooltip("El Tag del objeto que activar� el cambio de m�sica.")]
    [SerializeField] private string playerTag = "Player";


    void Awake()
    {
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            Debug.LogWarning($"El Collider en '{gameObject.name}' no est� marcado como 'Is Trigger'. Se recomienda marcarlo.", this);
        }

        if (musicAudioSource == null)
        {
            Debug.LogError($"Music Audio Source no est� asignado en '{gameObject.name}'!", this);
            enabled = false;
            return; // Salir temprano si falta configuraci�n cr�tica
        }
        // Guardar el volumen inicial como fallback por si acaso
        originalVolumeBeforeEnter = musicAudioSource.volume;

        if (areaSpecificMusic == null)
        {
            Debug.LogError($"Area Specific Music no est� asignado en '{gameObject.name}'!", this);
            enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!enabled || !other.CompareTag(playerTag))
        {
            return;
        }

        Debug.Log($"'{playerTag}' entr� en la zona '{gameObject.name}'. Intentando cambiar m�sica.");

        if (musicAudioSource.clip != areaSpecificMusic)
        {
            // Almacenar clip Y volumen originales ANTES de cambiar
            originalMusicBeforeEnter = musicAudioSource.clip;
            originalVolumeBeforeEnter = musicAudioSource.volume; // Guardar volumen actual
            Debug.Log($"M�sica original guardada: {(originalMusicBeforeEnter != null ? originalMusicBeforeEnter.name : "Ninguna")} (Vol: {originalVolumeBeforeEnter})");

            // Iniciar cambio, pasando el VOLUMEN ESPEC�FICO del �rea
            StartMusicChange(areaSpecificMusic, areaSpecificVolume);
        }
        else
        {
            Debug.Log("La m�sica espec�fica del �rea ya est� sonando.");
            // Opcional: �Ajustar volumen si ya estaba sonando pero a otro nivel?
            // if (Mathf.Abs(musicAudioSource.volume - areaSpecificVolume) > 0.01f) {
            //     StartCoroutine(FadeToVolume(areaSpecificVolume, fadeDuration));
            // }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!enabled || !other.CompareTag(playerTag))
        {
            return;
        }

        Debug.Log($"'{playerTag}' sali� de la zona '{gameObject.name}'. Intentando volver a m�sica anterior.");

        AudioClip clipToPlayOnExit = null;
        if (musicToReturnTo != null)
        {
            clipToPlayOnExit = musicToReturnTo;
            Debug.Log($"Volviendo a m�sica de retorno fija: {clipToPlayOnExit.name}");
        }
        else if (originalMusicBeforeEnter != null)
        {
            clipToPlayOnExit = originalMusicBeforeEnter;
            Debug.Log($"Volviendo a m�sica original antes de entrar: {clipToPlayOnExit.name}");
        }
        else
        {
            Debug.LogWarning("No hay m�sica definida a la que volver al salir de la zona.");
            return;
        }


        // Cambiar solo si no es ya la m�sica correcta Y el volumen no es ya el original
        if (musicAudioSource.clip != clipToPlayOnExit || Mathf.Abs(musicAudioSource.volume - originalVolumeBeforeEnter) > 0.01f)
        {
            // Iniciar cambio, pasando el VOLUMEN ORIGINAL guardado
            StartMusicChange(clipToPlayOnExit, originalVolumeBeforeEnter);
        }
        else
        {
            Debug.Log("La m�sica de salida deseada y su volumen ya est�n activos.");
        }
    }


    // Modificado para aceptar el volumen objetivo
    private void StartMusicChange(AudioClip newClip, float targetVolume)
    {
        if (newClip == null)
        {
            Debug.LogWarning("Se intent� cambiar a un AudioClip nulo.");
            // Podr�as decidir si detener la m�sica aqu� o no hacer nada
            // musicAudioSource.Stop();
            return;
        }

        // Asegurar que el volumen objetivo est� entre 0 y 1
        targetVolume = Mathf.Clamp01(targetVolume);

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Pasar el volumen objetivo a la corutina
        fadeCoroutine = StartCoroutine(FadeBetweenClips(newClip, fadeDuration, targetVolume));
    }


    // Modificado para aceptar y usar el volumen objetivo
    private IEnumerator FadeBetweenClips(AudioClip newClip, float duration, float targetVolume)
    {
        float startVolumeFadeOut = musicAudioSource.volume; // Volumen al iniciar el fade out
        float timer = 0f;

        // 1. Fade Out del clip actual
        if (musicAudioSource.isPlaying && duration > 0 && startVolumeFadeOut > 0)
        {
            Debug.Log("Iniciando Fade Out...");
            timer = 0f;
            while (timer < duration)
            {
                musicAudioSource.volume = Mathf.Lerp(startVolumeFadeOut, 0f, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        musicAudioSource.volume = 0f; // Asegurar que llega a cero


        // 2. Detener, cambiar clip y empezar el nuevo a volumen cero
        musicAudioSource.Stop();
        musicAudioSource.clip = newClip;
        musicAudioSource.volume = 0f;
        if (musicAudioSource.clip != null)
        {
            musicAudioSource.loop = true;
            musicAudioSource.Play();
            Debug.Log($"Reproduciendo nuevo clip: {newClip.name} a volumen objetivo: {targetVolume}");
        }


        // 3. Fade In del nuevo clip HASTA EL TARGET VOLUME
        if (musicAudioSource.isPlaying && duration > 0)
        {
            Debug.Log("Iniciando Fade In...");
            timer = 0f;
            while (timer < duration)
            {
                // Usar targetVolume en lugar de startVolumeFadeOut
                musicAudioSource.volume = Mathf.Lerp(0f, targetVolume, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        // Asegurar que alcanza exactamente el volumen objetivo, est� sonando o no (si duration=0)
        musicAudioSource.volume = targetVolume;


        Debug.Log("Cambio de m�sica completado.");
        fadeCoroutine = null;
    }

    // --- Opcional: Corutina separada solo para cambiar volumen si el clip ya est� sonando ---
    private IEnumerator FadeToVolume(float targetVolume, float duration)
    {
        float startVolume = musicAudioSource.volume;
        float timer = 0f;
        Debug.Log($"Iniciando Fade de volumen a {targetVolume}...");

        while (timer < duration)
        {
            musicAudioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        musicAudioSource.volume = targetVolume;
        Debug.Log("Fade de volumen completado.");
        fadeCoroutine = null; // Asumiendo que esta corutina tambi�n usa fadeCoroutine
    }
}