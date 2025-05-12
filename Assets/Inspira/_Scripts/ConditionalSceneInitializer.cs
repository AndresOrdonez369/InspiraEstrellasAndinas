using UnityEngine;
using UnityEngine.Playables;    // Necesario para PlayableDirector
using UnityEngine.SceneManagement; // Necesario para SceneManager
using System.Collections.Generic; // Necesario para List<>

public class ConditionalSceneInitializer : MonoBehaviour // Renombrado para reflejar mejor su prop�sito
{
    [Header("Configuraci�n de Condici�n")]
    [Tooltip("La clave de PlayerPrefs que indica la escena anterior.")]
    public string playerPrefsKey = "recuerdo";

    [Tooltip("El nombre EXACTO de la escena desde la que se debe venir.")]
    public string requiredPreviousSceneName = "Recuerdo_2"; // ��CAMBIA ESTO!!

    [Header("Elementos a Activar si se Cumple la Condici�n")]
    [Tooltip("Arrastra aqu� el GameObject que quieres ACTIVAR si se cumple la condici�n. Debe estar INACTIVO en la escena por defecto.")]
    public GameObject gameObjectToActivateOnCondition;

    [Tooltip("Arrastra aqu� el PlayableDirector de la Timeline a activar si se cumple la condici�n.")]
    public PlayableDirector specificTimelineDirector;

    [Header("Elementos a Desactivar si se Cumple la Condici�n")]
    [Tooltip("Arrastra aqu� los GameObjects que contienen las Timelines (u otros elementos) que NO deben activarse si se cumple la condici�n. Estos ser�n desactivados COMPLETAMENTE.")]
    public List<GameObject> gameObjectsToDisableOnCondition; // Cambiado de List<PlayableDirector> a List<GameObject> para m�s flexibilidad

    [Header("Opcional")]
    [Tooltip("Si se marca, la clave de PlayerPrefs se eliminar� despu�s de la activaci�n especial.")]
    public bool clearPlayerPrefsAfterActivation = true;

    void Awake()
    {
        bool conditionMet = false;

        // 1. Comprobar la condici�n de PlayerPrefs
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            string previousScene = PlayerPrefs.GetString(playerPrefsKey);
            Debug.Log($"{this.GetType().Name}: Se encontr� la clave '{playerPrefsKey}' con valor '{previousScene}'.");

            if (previousScene == requiredPreviousSceneName)
            {
                Debug.Log($"{this.GetType().Name}: Condici�n CUMPLIDA. Viniendo de '{requiredPreviousSceneName}'. Aplicando configuraci�n especial.");
                conditionMet = true;
            }
            else
            {
                Debug.Log($"{this.GetType().Name}: Condici�n NO cumplida. Se vino de '{previousScene}', no de '{requiredPreviousSceneName}'. Procediendo con inicio normal.");
            }
        }
        else
        {
            Debug.Log($"{this.GetType().Name}: No se encontr� la clave '{playerPrefsKey}'. Procediendo con inicio normal.");
        }

        // 2. Actuar seg�n la condici�n
        if (conditionMet)
        {
            // Desactivar los GameObjects que no deben ejecutarse en este escenario especial.
            // Esto se hace ANTES de activar los elementos especiales para evitar que sus Awake/Start se ejecuten.
            DisableOtherElements();

            // Activar el GameObject espec�fico
            if (gameObjectToActivateOnCondition != null)
            {
                gameObjectToActivateOnCondition.SetActive(true);
                Debug.Log($"{this.GetType().Name}: GameObject '{gameObjectToActivateOnCondition.name}' ACTIVADO.");
            }
            else
            {
                Debug.LogWarning($"{this.GetType().Name}: 'GameObject To Activate On Condition' no est� asignado.", this);
            }

            // Activar la timeline espec�fica
            if (specificTimelineDirector != null)
            {
                // Aseg�rate de que el GameObject del director est� activo si el director mismo lo est�
                if (!specificTimelineDirector.gameObject.activeSelf)
                {
                    specificTimelineDirector.gameObject.SetActive(true); // Podr�a estar dentro del gameObjectToActivateOnCondition
                }
                specificTimelineDirector.Play();
                Debug.Log($"{this.GetType().Name}: Timeline espec�fica '{specificTimelineDirector.playableAsset.name}' activada.");
            }
            else
            {
                Debug.LogWarning($"{this.GetType().Name}: 'Specific Timeline Director' no est� asignado.", this);
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
            // Asegurarse de que el GameObject especial NO est� activo si la condici�n no se cumple.
            // (Esto es por si acaso, pero deber�a estar inactivo por defecto).
            if (gameObjectToActivateOnCondition != null && gameObjectToActivateOnCondition.activeSelf)
            {
                gameObjectToActivateOnCondition.SetActive(false);
                Debug.Log($"{this.GetType().Name}: GameObject '{gameObjectToActivateOnCondition.name}' DESACTIVADO (inicio normal).");
            }

            // Los GameObjects en 'gameObjectsToDisableOnCondition' permanecer�n activos
            // y sus timelines con PlayOnAwake (si las tienen) se ejecutar�n normalmente.
            // Si tus timelines de inicio normal no usan PlayOnAwake, aqu� podr�as activarlas expl�citamente.
            Debug.Log($"{this.GetType().Name}: Procediendo con l�gica de inicio normal de la escena.");
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
                // Esto previene que CUALQUIER script en �l (incluidos PlayableDirectors con PlayOnAwake) se ejecute.
                go.SetActive(false);
                Debug.Log($"{this.GetType().Name}: GameObject '{go.name}' desactivado.");
            }
        }
    }

    // Start() ya no es necesario para la l�gica principal si todo est� en Awake()
    // void Start() { }

    // Podr�as tener un OnValidate para advertencias en el editor
    void OnValidate()
    {
        if (gameObjectToActivateOnCondition != null && gameObjectToActivateOnCondition.activeSelf)
        {
            Debug.LogWarning($"'{gameObjectToActivateOnCondition.name}' (asignado a 'GameObject To Activate On Condition') deber�a estar INACTIVO por defecto en la escena para que este script lo active condicionalmente.", gameObjectToActivateOnCondition);
        }
        // Puedes a�adir m�s validaciones aqu� para los otros campos si lo deseas
    }
}