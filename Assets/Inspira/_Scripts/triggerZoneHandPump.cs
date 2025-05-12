using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; // Necesario para PlayableDirector

[RequireComponent(typeof(Collider))] // Asegura que este objeto tenga un Collider
public class TriggerZone_WaitForTimelineAndPlayer : MonoBehaviour
{
    [Header("Timelines")]
    [Tooltip("El Timeline que se reproduce autom�ticamente al inicio (Play On Awake = true).")]
    [SerializeField] private PlayableDirector preliminaryDirector;

    [Tooltip("El Timeline que se activar� CUANDO el preliminar termine Y el jugador est� en la zona.")]
    [SerializeField] private PlayableDirector targetDirector; // Renombrado para claridad (era bombaIntroDirector)

    [Header("Trigger Settings")]
    [Tooltip("El Tag del objeto jugador.")]
    [SerializeField] private string playerTag = "Player";

    [SerializeField] private GameObject partibleFx = null;

    public List<GameObject> objetosCo2;

    // Estado interno para rastrear las condiciones
    private bool preliminaryHasFinished = false;
    private bool playerIsInZone = false;
    private bool targetTimelineHasPlayed = false; // Para asegurar que solo se reproduzca una vez

    // --- Suscripci�n a Eventos ---
    void OnEnable()
    {
        // Suscribirse al evento 'stopped' del director PRELIMINAR
        if (preliminaryDirector != null)
        {
            // Comprobar si ya termin� ANTES de suscribirnos (por si este script/objeto se activ� tarde)
            // Usamos una peque�a tolerancia por precisi�n de flotantes.
            if (preliminaryDirector.state != PlayState.Playing && preliminaryDirector.time >= preliminaryDirector.duration - Time.deltaTime)
            {
                preliminaryHasFinished = true;
                Debug.Log($"[{gameObject.name}] Preliminary Director ya hab�a terminado al habilitar este script.");
                // No intentar reproducir aqu�, esperar la condici�n del trigger tambi�n.
            }
            else
            {
                preliminaryDirector.stopped += OnPreliminaryTimelineStopped;
                Debug.Log($"[{gameObject.name}] Suscrito al evento 'stopped' de Preliminary Director.");
            }
        }
        else
        {
            Debug.LogError($"[{gameObject.name}] Preliminary Director no est� asignado en OnEnable!");
        }
    }

    void ActivarObjetos()
    {
        foreach (GameObject obj in objetosCo2)
        {
            obj.SetActive(true);
        }
    }
    void OnDisable()
    {
        // MUY IMPORTANTE: Desuscribirse
        if (preliminaryDirector != null)
        {
            preliminaryDirector.stopped -= OnPreliminaryTimelineStopped;
            Debug.Log($"[{gameObject.name}] Desuscrito del evento 'stopped' de Preliminary Director.");
        }
    }
    // -----------------------------

    void Start()
    {
        // Validar asignaciones
        if (preliminaryDirector == null)
        {
            Debug.LogError($"[{gameObject.name}] Preliminary Director no asignado!");
            enabled = false;
            return;
        }
        if (targetDirector == null)
        {
            Debug.LogError($"[{gameObject.name}] Target Director no asignado!");
            enabled = false;
            return;
        }

        // Asegurarse de la configuraci�n de PlayOnAwake
        if (!preliminaryDirector.playOnAwake && preliminaryDirector.state != PlayState.Playing)
        {
            Debug.LogWarning($"[{gameObject.name}] Preliminary Director no tiene Play On Awake y no est� sonando. Aseg�rate de que se inicie correctamente.");
        }
        targetDirector.playOnAwake = false; // El target NUNCA debe empezar solo

        // Asegurar que el collider es trigger
        Collider col = GetComponent<Collider>();
        if (col && !col.isTrigger)
        {
            Debug.LogWarning($"[{gameObject.name}] El Collider no era Trigger, se ha forzado a serlo.");
            col.isTrigger = true;
        }

        // Comprobaci�n adicional en Start por si termina extremadamente r�pido
        if (!preliminaryHasFinished && preliminaryDirector.state != PlayState.Playing && preliminaryDirector.time >= preliminaryDirector.duration - Time.deltaTime)
        {
            preliminaryHasFinished = true;
            Debug.Log($"[{gameObject.name}] Preliminary Director ya termin� en Start.");
            // No reproducir a�n, esperar al trigger.
        }
    }

    // --- Funci�n llamada cuando el PlayableDirector PRELIMINAR se detiene ---
    private void OnPreliminaryTimelineStopped(PlayableDirector director)
    {
        // Solo reaccionar si es nuestro director preliminar
        if (director == preliminaryDirector)
        {
            Debug.Log($"[{gameObject.name}] Evento 'stopped' recibido de Preliminary Timeline.");
            preliminaryHasFinished = true;

            // �Ahora comprobar si el jugador YA EST� en la zona!
            CheckConditionsAndPlayTarget();
        }
    }

    // --- Funciones del Trigger ---
    private void OnTriggerEnter(Collider other)
    {
        // Solo reaccionar si es el jugador
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"[{gameObject.name}] Jugador entr� en la zona.");
            playerIsInZone = true;

            // �Ahora comprobar si el timeline preliminar YA HA TERMINADO!
            CheckConditionsAndPlayTarget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si el jugador sale, ya no cumple una de las condiciones
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"[{gameObject.name}] Jugador sali� de la zona.");
            playerIsInZone = false;
        }
    }

    // --- L�gica central para verificar AMBAS condiciones ---
    private void CheckConditionsAndPlayTarget()
    {
        // Verificar las TRES condiciones: Preliminar terminado Y Jugador en zona Y Target no ha sonado a�n
        if (preliminaryHasFinished && playerIsInZone && !targetTimelineHasPlayed)
        {
            Debug.Log($"[{gameObject.name}] �CONDICIONES CUMPLIDAS! Iniciando Target Director: {targetDirector.name}");
            targetTimelineHasPlayed = true; // Marcar para que no se repita

            if (targetDirector != null)
            {
                partibleFx.SetActive(false);
                ActivarObjetos();
                targetDirector.Play();


            }
            else
            {
                Debug.LogError($"[{gameObject.name}] Target Director es nulo al intentar reproducirlo!");
            }

            // Opcional: Desactivar este componente o el trigger si es un evento �nico
            // this.enabled = false;
            // GetComponent<Collider>().enabled = false;
        }
        else
        {
            // Log para depuraci�n si las condiciones no se cumplen (y a�n no ha sonado)
            if (!targetTimelineHasPlayed)
            {
                //Debug.Log($"[{gameObject.name}] Condiciones NO cumplidas: Preliminar Terminado={preliminaryHasFinished}, Jugador en Zona={playerIsInZone}");
            }
        }
    }
}