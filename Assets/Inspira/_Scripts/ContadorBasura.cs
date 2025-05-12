using UnityEngine;
using UnityEngine.Playables; // Necesario para PlayableDirector

// Aseg�rate de que el GameObject tenga un componente AudioSource
[RequireComponent(typeof(AudioSource))]
public class ContadorBasura : MonoBehaviour
{
    [Header("Configuraci�n de Detecci�n")]
    [Tooltip("La Tag que deben tener los objetos de basura.")]
    public string tagBasura = "Basura"; // Aseg�rate que coincida con la Tag creada

    [Tooltip("Cu�ntos objetos de basura se necesitan para activar la timeline.")]
    public int basuraNecesaria = 6;

    [Header("Efectos")]
    [Tooltip("El Prefab del sistema de part�culas a instanciar.")]
    public GameObject prefabParticula;

    [Tooltip("El PlayableDirector que controla la Timeline final.")]
    public PlayableDirector timelineFinal;

    [Tooltip("Punto opcional donde instanciar las part�culas. Si es nulo, se usar� la posici�n de este objeto.")]
    public Transform puntoSpawnParticulas;

    // --- NUEVO: Configuraci�n de Sonido ---
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
            // El RequireComponent deber�a prevenir esto, pero es una buena pr�ctica verificar
            Debug.LogError("�No se encontr� un componente AudioSource! Aseg�rate de que est� a�adido al GameObject.", this);
        }
        // -----------------------------------------------

        // Validaciones existentes (movidas a Awake o mantenidas en Start si prefieres)
        if (timelineFinal == null)
        {
            Debug.LogError("El campo 'Timeline Final' no est� asignado en el script ContadorBasura.", this);
        }
        if (prefabParticula == null)
        {
            Debug.LogWarning("El campo 'Prefab Particula' no est� asignado en el script ContadorBasura.", this);
        }
        // --- NUEVO: Validaci�n del AudioClip ---
        if (sonidoDeteccion == null)
        {
            Debug.LogWarning("No se ha asignado un 'Sonido Deteccion' en el script ContadorBasura. No se reproducir� sonido al detectar basura.", this);
        }
        // ---------------------------------------
    }


    // Se llama autom�ticamente cuando otro Collider entra en este Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Si la timeline final ya se activ�, no hacemos nada m�s
        if (timelineActivada)
        {
            return;
        }

        // Comprobamos si el objeto que entr� tiene la Tag correcta
        if (other.CompareTag(tagBasura))
        {
            contadorActual++;
            Debug.Log($"�Basura detectada! Objeto: {other.name}. Contador: {contadorActual}/{basuraNecesaria}");

            // --- NUEVO: Reproducir Sonido ---
            if (audioSource != null && sonidoDeteccion != null)
            {
                // Usamos PlayOneShot para permitir que el sonido se reproduzca
                // varias veces si entra basura muy r�pido, sin cortarse a s� mismo.
                audioSource.PlayOneShot(sonidoDeteccion);
                Debug.Log("Sonido de detecci�n reproducido.");
            }
            // --------------------------------

            // 1. Instanciar y reproducir part�cula (si est� asignada)
            if (prefabParticula != null)
            {
                Vector3 spawnPosition = (puntoSpawnParticulas != null) ? puntoSpawnParticulas.position : transform.position;
                Instantiate(prefabParticula, spawnPosition, Quaternion.identity);
                Debug.Log("Part�cula instanciada.");
            }
            else
            {
                Debug.LogWarning("No hay prefab de part�cula asignado al basurero.", this);
            }

            // 2. Destruir el objeto de basura que entr�
            Destroy(other.gameObject);
            Debug.Log($"Objeto '{other.name}' destruido.");

            // 3. Comprobar si hemos alcanzado el n�mero necesario
            if (contadorActual >= basuraNecesaria)
            {
                Debug.Log("�Contador de basura alcanzado!");
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
            Debug.LogError("Se intent� activar la timeline final, �pero no est� asignada en el Inspector!", this);
        }
    }

    // Start ya no es estrictamente necesario para las validaciones si se hacen en Awake,
    // pero puedes dejarlo si tienes otra l�gica de inicializaci�n aqu�.
    // void Start() { }
}