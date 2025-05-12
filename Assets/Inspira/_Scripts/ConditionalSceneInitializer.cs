using UnityEngine;
using UnityEngine.Playables;    // Necesario para PlayableDirector
using UnityEngine.SceneManagement; // Necesario para SceneManager
using System.Collections.Generic; // Necesario para List<>

public class ConditionalSceneInitializer : MonoBehaviour // Renombrado para reflejar mejor su propósito
{
    [Header("Configuración de Condición")]
    [Tooltip("La clave de PlayerPrefs que indica la escena anterior.")]
    public string playerPrefsKey = "recuerdo";

    [Tooltip("El nombre EXACTO de la escena desde la que se debe venir.")]
    public string requiredPreviousSceneName = "Recuerdo_2"; // ¡¡CAMBIA ESTO!!

    [Header("Elementos a Activar si se Cumple la Condición")]
    [Tooltip("Arrastra aquí el GameObject que quieres ACTIVAR si se cumple la condición. Debe estar INACTIVO en la escena por defecto.")]
    public GameObject gameObjectToActivateOnCondition;

    [Tooltip("Arrastra aquí el PlayableDirector de la Timeline a activar si se cumple la condición.")]
    public PlayableDirector specificTimelineDirector;

    [Header("Elementos a Desactivar si se Cumple la Condición")]
    [Tooltip("Arrastra aquí los GameObjects que contienen las Timelines (u otros elementos) que NO deben activarse si se cumple la condición. Estos serán desactivados COMPLETAMENTE.")]
    public List<GameObject> gameObjectsToDisableOnCondition; // Cambiado de List<PlayableDirector> a List<GameObject> para más flexibilidad

    [Header("Opcional")]
    [Tooltip("Si se marca, la clave de PlayerPrefs se eliminará después de la activación especial.")]
    public bool clearPlayerPrefsAfterActivation = true;

    void Awake()
    {
        bool conditionMet = false;

        // 1. Comprobar la condición de PlayerPrefs
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            string previousScene = PlayerPrefs.GetString(playerPrefsKey);
            Debug.Log($"{this.GetType().Name}: Se encontró la clave '{playerPrefsKey}' con valor '{previousScene}'.");

            if (previousScene == requiredPreviousSceneName)
            {
                Debug.Log($"{this.GetType().Name}: Condición CUMPLIDA. Viniendo de '{requiredPreviousSceneName}'. Aplicando configuración especial.");
                conditionMet = true;
            }
            else
            {
                Debug.Log($"{this.GetType().Name}: Condición NO cumplida. Se vino de '{previousScene}', no de '{requiredPreviousSceneName}'. Procediendo con inicio normal.");
            }
        }
        else
        {
            Debug.Log($"{this.GetType().Name}: No se encontró la clave '{playerPrefsKey}'. Procediendo con inicio normal.");
        }

        // 2. Actuar según la condición
        if (conditionMet)
        {
            // Desactivar los GameObjects que no deben ejecutarse en este escenario especial.
            // Esto se hace ANTES de activar los elementos especiales para evitar que sus Awake/Start se ejecuten.
            DisableOtherElements();

            // Activar el GameObject específico
            if (gameObjectToActivateOnCondition != null)
            {
                gameObjectToActivateOnCondition.SetActive(true);
                Debug.Log($"{this.GetType().Name}: GameObject '{gameObjectToActivateOnCondition.name}' ACTIVADO.");
            }
            else
            {
                Debug.LogWarning($"{this.GetType().Name}: 'GameObject To Activate On Condition' no está asignado.", this);
            }

            // Activar la timeline específica
            if (specificTimelineDirector != null)
            {
                // Asegúrate de que el GameObject del director esté activo si el director mismo lo está
                if (!specificTimelineDirector.gameObject.activeSelf)
                {
                    specificTimelineDirector.gameObject.SetActive(true); // Podría estar dentro del gameObjectToActivateOnCondition
                }
                specificTimelineDirector.Play();
                Debug.Log($"{this.GetType().Name}: Timeline específica '{specificTimelineDirector.playableAsset.name}' activada.");
            }
            else
            {
                Debug.LogWarning($"{this.GetType().Name}: 'Specific Timeline Director' no está asignado.", this);
            }

            // Opcional: Limpiar la clave de PlayerPrefs
            if (clearPlayerPrefsAfterActivation)
            {
                PlayerPrefs.DeleteKey(playerPrefsKey);
                PlayerPrefs.Save();
                Debug.Log($"{this.GetType().Name}: Clave '{playerPrefsKey}' eliminada de PlayerPrefs.");
            }
        }
        else
        {
            // Inicio Normal:
            // Asegurarse de que el GameObject especial NO esté activo si la condición no se cumple.
            // (Esto es por si acaso, pero debería estar inactivo por defecto).
            if (gameObjectToActivateOnCondition != null && gameObjectToActivateOnCondition.activeSelf)
            {
                gameObjectToActivateOnCondition.SetActive(false);
                Debug.Log($"{this.GetType().Name}: GameObject '{gameObjectToActivateOnCondition.name}' DESACTIVADO (inicio normal).");
            }

            // Los GameObjects en 'gameObjectsToDisableOnCondition' permanecerán activos
            // y sus timelines con PlayOnAwake (si las tienen) se ejecutarán normalmente.
            // Si tus timelines de inicio normal no usan PlayOnAwake, aquí podrías activarlas explícitamente.
            Debug.Log($"{this.GetType().Name}: Procediendo con lógica de inicio normal de la escena.");
        }
    }

    void DisableOtherElements()
    {
        if (gameObjectsToDisableOnCondition == null || gameObjectsToDisableOnCondition.Count == 0)
        {
            //Debug.Log($"{this.GetType().Name}: No hay otros GameObjects configurados para desactivar.");
            return;
        }

        Debug.Log($"{this.GetType().Name}: Desactivando {gameObjectsToDisableOnCondition.Count} otros GameObjects...");
        foreach (GameObject go in gameObjectsToDisableOnCondition)
        {
            if (go != null)
            {
                // Desactivar el GameObject completo.
                // Esto previene que CUALQUIER script en él (incluidos PlayableDirectors con PlayOnAwake) se ejecute.
                go.SetActive(false);
                Debug.Log($"{this.GetType().Name}: GameObject '{go.name}' desactivado.");
            }
        }
    }

    // Start() ya no es necesario para la lógica principal si todo está en Awake()
    // void Start() { }

    // Podrías tener un OnValidate para advertencias en el editor
    void OnValidate()
    {
        if (gameObjectToActivateOnCondition != null && gameObjectToActivateOnCondition.activeSelf)
        {
            Debug.LogWarning($"'{gameObjectToActivateOnCondition.name}' (asignado a 'GameObject To Activate On Condition') debería estar INACTIVO por defecto en la escena para que este script lo active condicionalmente.", gameObjectToActivateOnCondition);
        }
        // Puedes añadir más validaciones aquí para los otros campos si lo deseas
    }
}