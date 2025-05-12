using UnityEngine;
using UnityEngine.Playables; // Necesario para PlayableDirector
using UnityEngine.Events;    // Necesario para UnityEvent

public class TimelineTriggerZone : MonoBehaviour
{
    [Header("Timeline Setup")]
    [SerializeField] private PlayableDirector timelineDirector; // El director que controla la voz/animaci�n actual

    [Header("Character Control")]
    [SerializeField] private GameObject characterToHide; // Arrastra aqu� el GameObject del personaje que habla AHORA
    [SerializeField] private GameObject characterToShow; // Arrastra aqu� el GameObject del NUEVO personaje

    [Header("Trigger Setup")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool playOnce = true;

    // Evento opcional para encadenar acciones (como iniciar un segundo timeline)
    [Header("Optional: Actions After Timeline")]
    public UnityEvent onTimelineComplete;

    private bool hasBeenTriggered = false;
    private bool isTimelinePlaying = false; // Para saber si el evento 'stopped' es por finalizaci�n

    // --- Suscripci�n a Eventos ---
    void OnEnable()
    {
        if (timelineDirector != null)
        {
            // Suscribirse al evento 'stopped' del director
            timelineDirector.stopped += OnTimelineStopped;
        }
    }

    void OnDisable()
    {
        if (timelineDirector != null)
        {
            // MUY IMPORTANTE: Desuscribirse para evitar errores y memory leaks
            timelineDirector.stopped -= OnTimelineStopped;
        }
    }
    // -----------------------------

    void Start()
    {
        // Asegurarse de que el estado inicial es correcto
        if (timelineDirector == null)
        {
            Debug.LogError("Timeline Director no asignado en " + gameObject.name);
            enabled = false;
            return;
        }
        if (characterToHide == null)
        {
            Debug.LogWarning("Character To Hide no asignado en " + gameObject.name);
        }
        if (characterToShow == null)
        {
            Debug.LogWarning("Character To Show no asignado en " + gameObject.name);
        }

        // Asegurar estado inicial de los personajes (opcional, pero bueno para claridad)
        // Asume que ya est�n configurados correctamente en la escena,
        // pero podr�as forzarlo aqu� si lo necesitas.
        // if (characterToHide != null) characterToHide.SetActive(true); // O el estado que deba tener al inicio
        // if (characterToShow != null) characterToShow.SetActive(false);

        timelineDirector.Stop(); // Asegurar que no est� corriendo
        timelineDirector.time = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (playOnce && hasBeenTriggered)
            {
                return;
            }

            if (timelineDirector != null && !isTimelinePlaying) // Evitar reiniciar si ya est� sonando
            {
                Debug.Log("Player entr�. Iniciando Timeline: " + timelineDirector.playableAsset.name);
                // IMPORTANTE: Asegurarse de que los personajes est�n en el estado correcto ANTES de empezar
                if (characterToHide != null) characterToHide.SetActive(true);
                if (characterToShow != null) characterToShow.SetActive(false);

                timelineDirector.Stop(); // Detener por si acaso
                timelineDirector.time = 0; // Reiniciar al inicio
                timelineDirector.Play();
                isTimelinePlaying = true; // Marcar que hemos iniciado la reproducci�n

                hasBeenTriggered = true;

                // (Opcional) Desactivar trigger ya no es necesario aqu� si usamos 'hasBeenTriggered'
            }
        }
    }

    // --- Esta funci�n se llamar� cuando el PlayableDirector se detenga ---
    private void OnTimelineStopped(PlayableDirector director)
    {
        // Nos aseguramos de que sea NUESTRO director el que se detuvo
        // y que se detuvo porque nosotros lo iniciamos y lleg� al final (o fue detenido)
        if (director == timelineDirector && isTimelinePlaying)
        {
            Debug.Log("Timeline " + director.playableAsset.name + " finalizado o detenido.");

            // Realizar las acciones de cambio de personaje
            if (characterToHide != null)
            {
                Debug.Log("Ocultando personaje: " + characterToHide.name);
                characterToHide.SetActive(false);
            }
            if (characterToShow != null)
            {
                Debug.Log("Mostrando personaje: " + characterToShow.name);
                characterToShow.SetActive(true);
                // Aqu� podr�as iniciar la animaci�n o el timeline del nuevo personaje si fuera necesario
                // Ejemplo: nuevoPersonajeAnimator.Play("SuAnimacionIdle");
                // O si el nuevo personaje tiene su propio PlayableDirector:
                // PlayableDirector nuevoDirector = characterToShow.GetComponent<PlayableDirector>();
                // if (nuevoDirector != null) nuevoDirector.Play();
            }

            // Invocar el evento UnityEvent si hay algo suscrito en el Inspector
            onTimelineComplete.Invoke();

            isTimelinePlaying = false; // Reiniciar la bandera
        }
    }

    // (Opcional) OnTriggerExit si necesitas l�gica al salir
    // private void OnTriggerExit(Collider other) { ... }
}