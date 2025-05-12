using UnityEngine;
using UnityEngine.Playables; // Necesario para PlayableDirector
using System.Collections.Generic; // Si necesitas la lista de objetosCo2, aunque para el flujo actual no parece necesaria aquí.

[RequireComponent(typeof(Collider))]
public class RuthSequenceInitiator : MonoBehaviour
{
    [Header("Timelines & Control")]
    [Tooltip("OPCIONAL: Timeline que DEBE terminar ANTES de que este trigger pueda actuar.")]
    [SerializeField] private PlayableDirector preliminaryDirector; // Timeline que debe terminar primero

    [Tooltip("El Timeline de bienvenida/explicación de Ruth que se activará.")]
    [SerializeField] private PlayableDirector ruthWelcomeExplanationTimeline; // Este es el timeline que inicia los diálogos de Ruth y activa rocas/tooltips

   /* [Tooltip("Referencia al SimpleGameManager para coordinar acciones post-dialogo.")]
    [SerializeField] private SimpleGameManager simpleGameManager; // Referencia al manager principal*/

    [Header("Trigger Settings")]
    [Tooltip("El Tag del objeto jugador.")]
    [SerializeField] private string playerTag = "Player";

    [Header("VFX Control")]
    [Tooltip("El GameObject de efectos de partículas a desactivar.")]
    [SerializeField] private GameObject particleFxToDeactivate = null;

    // Estado interno
    private bool preliminaryHasFinishedOrNotSet = false; // Si no hay preliminary, se considera terminado
    private bool playerIsInZone = false;
    private bool ruthTimelineHasPlayed = false;

    void OnEnable()
    {
        if (preliminaryDirector != null)
        {
            // Usamos una pequeña tolerancia por precisión de flotantes.
            if (preliminaryDirector.state != PlayState.Playing && preliminaryDirector.time >= preliminaryDirector.duration - Time.deltaTime)
            {
                preliminaryHasFinishedOrNotSet = true;
                Debug.Log($"[{gameObject.name}] Preliminary Director ya había terminado al habilitar este script.");
            }
            else
            {
                preliminaryDirector.stopped += OnPreliminaryTimelineStopped;
                Debug.Log($"[{gameObject.name}] Suscrito al evento 'stopped' de Preliminary Director: {preliminaryDirector.name}");
            }
        }
        else
        {
            preliminaryHasFinishedOrNotSet = true; // Si no hay timeline preliminar, se considera "terminado" por defecto.
            Debug.Log($"[{gameObject.name}] No hay Preliminary Director asignado, se considera condición cumplida.");
        }
    }

    void OnDisable()
    {
        if (preliminaryDirector != null)
        {
            preliminaryDirector.stopped -= OnPreliminaryTimelineStopped;
            Debug.Log($"[{gameObject.name}] Desuscrito del evento 'stopped' de Preliminary Director: {preliminaryDirector.name}");
        }
    }

    void Start()
    {
        // Validaciones
        if (ruthWelcomeExplanationTimeline == null)
        {
            Debug.LogError($"[{gameObject.name}] Ruth Welcome/Explanation Timeline no asignado!");
            enabled = false;
            return;
        }
       /* if (simpleGameManager == null)
        {
            Debug.LogError($"[{gameObject.name}] SimpleGameManager no asignado!");
            enabled = false;
            return;
        }*/

        // El timeline de Ruth NO debe empezar solo
        ruthWelcomeExplanationTimeline.playOnAwake = false;

        // Asegurar que el collider es trigger
        Collider col = GetComponent<Collider>();
        if (col && !col.isTrigger)
        {
            Debug.LogWarning($"[{gameObject.name}] El Collider no era Trigger, se ha forzado a serlo.");
            col.isTrigger = true;
        }

        // Comprobación adicional en Start por si el preliminar termina muy rápido
        if (preliminaryDirector != null && !preliminaryHasFinishedOrNotSet &&
            preliminaryDirector.state != PlayState.Playing && preliminaryDirector.time >= preliminaryDirector.duration - Time.deltaTime)
        {
            preliminaryHasFinishedOrNotSet = true;
            Debug.Log($"[{gameObject.name}] Preliminary Director ya terminó en Start.");
        }
    }

    private void OnPreliminaryTimelineStopped(PlayableDirector director)
    {
        if (director == preliminaryDirector)
        {
            Debug.Log($"[{gameObject.name}] Evento 'stopped' recibido de Preliminary Timeline: {director.name}");
            preliminaryHasFinishedOrNotSet = true;
            CheckConditionsAndPlayRuthTimeline();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"[{gameObject.name}] Jugador ({playerTag}) entró en la zona.");
            playerIsInZone = true;
            CheckConditionsAndPlayRuthTimeline();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"[{gameObject.name}] Jugador ({playerTag}) salió de la zona.");
            playerIsInZone = false;
        }
    }

    private void CheckConditionsAndPlayRuthTimeline()
    {
        if (preliminaryHasFinishedOrNotSet && playerIsInZone && !ruthTimelineHasPlayed)
        {
            Debug.Log($"[{gameObject.name}] ¡CONDICIONES CUMPLIDAS! Iniciando Ruth Welcome/Explanation Timeline: {ruthWelcomeExplanationTimeline.name}");
            ruthTimelineHasPlayed = true;

            // Desactivar Partículas VFX
            if (particleFxToDeactivate != null)
            {
                Debug.Log($"[{gameObject.name}] Desactivando Particle FX: {particleFxToDeactivate.name}");
                particleFxToDeactivate.SetActive(false);
            }

            // Iniciar el Timeline de Ruth
            
            ruthWelcomeExplanationTimeline.Play();

            // No necesitamos `ActivarObjetos()` aquí porque el timeline de Ruth
            // tendrá signals para llamar a `simpleGameManager.ActivateRocksAndTooltips()` etc.

            // Opcional: Desactivar este componente o el trigger si es un evento único para este timeline
            // this.enabled = false; // Desactiva este script para no volver a comprobar
            // GetComponent<Collider>().enabled = false; // Desactiva el trigger completamente
        }
        else
        {
            if (!ruthTimelineHasPlayed)
            {
                //Debug.Log($"[{gameObject.name}] Condiciones para Ruth Timeline NO cumplidas: Preliminar Terminado={preliminaryHasFinishedOrNotSet}, Jugador en Zona={playerIsInZone}");
            }
        }
    }
}