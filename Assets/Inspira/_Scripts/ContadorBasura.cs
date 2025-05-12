using UnityEngine;
using UnityEngine.Playables; // Necesario para PlayableDirector

// Asegúrate de que el GameObject tenga un componente AudioSource
[RequireComponent(typeof(AudioSource))]
public class ContadorBasura : MonoBehaviour
{
    [Header("Configuración de Detección")]
    [Tooltip("La Tag que deben tener los objetos de basura.")]
    public string tagBasura = "Basura"; // Asegúrate que coincida con la Tag creada

    [Tooltip("Cuántos objetos de basura se necesitan para activar la timeline.")]
    public int basuraNecesaria = 6;

    [Header("Efectos")]
    [Tooltip("El Prefab del sistema de partículas a instanciar.")]
    public GameObject prefabParticula;

    [Tooltip("El PlayableDirector que controla la Timeline final.")]
    public PlayableDirector timelineFinal;

    [Tooltip("Punto opcional donde instanciar las partículas. Si es nulo, se usará la posición de este objeto.")]
    public Transform puntoSpawnParticulas;

    // --- NUEVO: Configuración de Sonido ---
    [Header("Sonido")]
    [Tooltip("El clip de audio a reproducir al detectar basura.")]
    public AudioClip sonidoDeteccion;
    // --------------------------------------

    // Contador interno
    private int contadorActual = 0;
    private bool timelineActivada = false;

    // --- NUEVO: Referencia al AudioSource ---
    private AudioSource audioSource;
    // --------------------------------------

    // Awake se llama antes que Start, ideal para obtener componentes
    void Awake()
    {
        // --- NUEVO: Obtener el componente AudioSource ---
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // El RequireComponent debería prevenir esto, pero es una buena práctica verificar
            Debug.LogError("¡No se encontró un componente AudioSource! Asegúrate de que esté añadido al GameObject.", this);
        }
        // -----------------------------------------------

        // Validaciones existentes (movidas a Awake o mantenidas en Start si prefieres)
        if (timelineFinal == null)
        {
            Debug.LogError("El campo 'Timeline Final' no está asignado en el script ContadorBasura.", this);
        }
        if (prefabParticula == null)
        {
            Debug.LogWarning("El campo 'Prefab Particula' no está asignado en el script ContadorBasura.", this);
        }
        // --- NUEVO: Validación del AudioClip ---
        if (sonidoDeteccion == null)
        {
            Debug.LogWarning("No se ha asignado un 'Sonido Deteccion' en el script ContadorBasura. No se reproducirá sonido al detectar basura.", this);
        }
        // ---------------------------------------
    }


    // Se llama automáticamente cuando otro Collider entra en este Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Si la timeline final ya se activó, no hacemos nada más
        if (timelineActivada)
        {
            return;
        }

        // Comprobamos si el objeto que entró tiene la Tag correcta
        if (other.CompareTag(tagBasura))
        {
            contadorActual++;
            Debug.Log($"¡Basura detectada! Objeto: {other.name}. Contador: {contadorActual}/{basuraNecesaria}");

            // --- NUEVO: Reproducir Sonido ---
            if (audioSource != null && sonidoDeteccion != null)
            {
                // Usamos PlayOneShot para permitir que el sonido se reproduzca
                // varias veces si entra basura muy rápido, sin cortarse a sí mismo.
                audioSource.PlayOneShot(sonidoDeteccion);
                Debug.Log("Sonido de detección reproducido.");
            }
            // --------------------------------

            // 1. Instanciar y reproducir partícula (si está asignada)
            if (prefabParticula != null)
            {
                Vector3 spawnPosition = (puntoSpawnParticulas != null) ? puntoSpawnParticulas.position : transform.position;
                Instantiate(prefabParticula, spawnPosition, Quaternion.identity);
                Debug.Log("Partícula instanciada.");
            }
            else
            {
                Debug.LogWarning("No hay prefab de partícula asignado al basurero.", this);
            }

            // 2. Destruir el objeto de basura que entró
            Destroy(other.gameObject);
            Debug.Log($"Objeto '{other.name}' destruido.");

            // 3. Comprobar si hemos alcanzado el número necesario
            if (contadorActual >= basuraNecesaria)
            {
                Debug.Log("¡Contador de basura alcanzado!");
                ActivarTimelineFinal();
            }
        }
    }

    private void ActivarTimelineFinal()
    {
        if (!timelineActivada && timelineFinal != null)
        {
            Debug.Log($"Activando Timeline: {timelineFinal.name}");
            timelineFinal.Play();
            timelineActivada = true;
        }
        else if (timelineFinal == null)
        {
            Debug.LogError("Se intentó activar la timeline final, ¡pero no está asignada en el Inspector!", this);
        }
    }

    // Start ya no es estrictamente necesario para las validaciones si se hacen en Awake,
    // pero puedes dejarlo si tienes otra lógica de inicialización aquí.
    // void Start() { }
}