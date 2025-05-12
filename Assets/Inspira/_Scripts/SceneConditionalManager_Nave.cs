using UnityEngine;
using UnityEngine.Playables; // Necesario para PlayableDirector
using System.Collections.Generic; // Necesario para List<>

public class SceneConditionalManager_Nave : MonoBehaviour
{
    [Header("Configuración de Cinemática Final (desde Recuerdo_2)")]
    [Tooltip("La clave de PlayerPrefs (string) que indica si se debe activar la cinemática final.")]
    public string playerPrefsKeyCinematic = "activarCinematicaFinal"; // Ej: se setea a "true" o un nombre de escena
    [Tooltip("El valor esperado en PlayerPrefs para activar la cinemática. Ej: 'Recuerdo_2' o 'true'.")]
    public string requiredValueForCinematic = "Recuerdo_2"; // ¡CAMBIA ESTO AL VALOR QUE USES DESDE RECUERDO_2!

    [Tooltip("GameObject que se activará para la cinemática final. Debe estar INACTIVO por defecto.")]
    public GameObject gameObjectForFinalCinematic;
    [Tooltip("PlayableDirector de la Timeline de la cinemática final.")]
    public PlayableDirector finalCinematicDirector;
    [Tooltip("OBjeto a activar al iniciar la cinematica final")]
    public GameObject objectMedal;
    [Tooltip("GameObjects a DESACTIVAR si se activa la cinemática final (ej: controles de jugador, UI normal, etc.).")]
    public List<GameObject> gameObjectsToDisableForFinalCinematic;
    [Tooltip("Si se marca, la clave de PlayerPrefs de la cinemática se eliminará después de su uso.")]
    public bool clearCinematicKeyAfterUse = true;

    [Header("Configuración de Dificultad")]
    [Tooltip("La clave de PlayerPrefs (int) para la dificultad.")]
    public string playerPrefsKeyDifficulty = "dificultadNave"; // Ej: "dificultad"
    [Tooltip("GameObject que se activará si la dificultad es 1. Debe estar INACTIVO por defecto.")]
    public GameObject gameObjectForDifficulty1;
    [Tooltip("GameObject que se activará si la dificultad es 2 o 3. Debe estar INACTIVO por defecto.")]
    public GameObject gameObjectForDifficulty2Or3;
    [Tooltip("Si se marca, la clave de PlayerPrefs de dificultad se eliminará después de su uso.")]
    public bool clearDifficultyKeyAfterUse = true;


    void Awake()
    {
        // --- PASO 0: Asegurar que los GameObjects controlados estén desactivados inicialmente ---
        // Esto es una salvaguarda por si se dejan activos en el editor.
        if (gameObjectForFinalCinematic != null) gameObjectForFinalCinematic.SetActive(false);
        if (gameObjectForDifficulty1 != null) gameObjectForDifficulty1.SetActive(false);
        if (gameObjectForDifficulty2Or3 != null) gameObjectForDifficulty2Or3.SetActive(false);
        // Nota: finalCinematicDirector.gameObject también debería estar inactivo si es un GO separado,
        // o ser parte de gameObjectForFinalCinematic.

        bool cinematicConditionMet = false;

        // --- PASO 1: Comprobar la condición de la CINEMÁTICA FINAL ---
        if (PlayerPrefs.HasKey(playerPrefsKeyCinematic))
        {
            string cinematicTriggerValue = PlayerPrefs.GetString(playerPrefsKeyCinematic);
            Debug.Log($"'{this.GetType().Name}': Se encontró la clave '{playerPrefsKeyCinematic}' con valor '{cinematicTriggerValue}'.");

            if (cinematicTriggerValue == requiredValueForCinematic)
            {
                Debug.Log($"'{this.GetType().Name}': Condición de CINEMÁTICA FINAL CUMPLIDA. Viniendo con la señal '{requiredValueForCinematic}'.");
                cinematicConditionMet = true;

                objectMedal.SetActive(true);
                // Desactivar elementos que no deben estar durante la cinemática
                DisableGameObjects(gameObjectsToDisableForFinalCinematic, "cinemática final");

                // Activar GameObject de la cinemática
                if (gameObjectForFinalCinematic != null)
                {
                    gameObjectForFinalCinematic.SetActive(true);
                    Debug.Log($"'{this.GetType().Name}': GameObject '{gameObjectForFinalCinematic.name}' ACTIVADO para cinemática final.");
                }
                else
                {
                    Debug.LogWarning($"'{this.GetType().Name}': 'GameObject For Final Cinematic' no está asignado.", this);
                }

                // Activar Timeline de la cinemática
                if (finalCinematicDirector != null)
                {
                    if (!finalCinematicDirector.gameObject.activeSelf)
                    {
                        finalCinematicDirector.gameObject.SetActive(true);
                    }
                    finalCinematicDirector.Play();
                    Debug.Log($"'{this.GetType().Name}': Timeline '{finalCinematicDirector.playableAsset.name}' ACTIVADA para cinemática final.");
                }
                else
                {
                    Debug.LogWarning($"'{this.GetType().Name}': 'Final Cinematic Director' no está asignado.", this);
                }

                if (clearCinematicKeyAfterUse)
                {
                    PlayerPrefs.DeleteKey(playerPrefsKeyCinematic);
                    PlayerPrefs.Save();
                    Debug.Log($"'{this.GetType().Name}': Clave '{playerPrefsKeyCinematic}' eliminada.");
                }
            }
            else
            {
                Debug.Log($"'{this.GetType().Name}': Condición de CINEMÁTICA FINAL NO CUMPLIDA. Valor '{cinematicTriggerValue}' no es '{requiredValueForCinematic}'.");
            }
        }
        else
        {
            Debug.Log($"'{this.GetType().Name}': No se encontró la clave '{playerPrefsKeyCinematic}' para la cinemática final.");
        }


        // --- PASO 2: Si NO se activó la cinemática final, comprobar la DIFICULTAD ---
        if (!cinematicConditionMet)
        {
            Debug.Log($"'{this.GetType().Name}': Procediendo a comprobar configuración de dificultad.");
            if (PlayerPrefs.HasKey(playerPrefsKeyDifficulty))
            {
                int difficultyValue = PlayerPrefs.GetInt(playerPrefsKeyDifficulty);
                Debug.Log($"'{this.GetType().Name}': Se encontró la clave '{playerPrefsKeyDifficulty}' con valor '{difficultyValue}'.");

                if (difficultyValue == 1)
                {
                    if (gameObjectForDifficulty1 != null)
                    {
                        gameObjectForDifficulty1.SetActive(true);
                        Debug.Log($"'{this.GetType().Name}': GameObject '{gameObjectForDifficulty1.name}' ACTIVADO para dificultad 1.");
                    }
                    else
                    {
                        Debug.LogWarning($"'{this.GetType().Name}': 'GameObject For Difficulty 1' no asignado, pero PlayerPrefs '{playerPrefsKeyDifficulty}' era 1.", this);
                    }
                }
                else if (difficultyValue == 2 || difficultyValue == 3)
                {
                    if (gameObjectForDifficulty2Or3 != null)
                    {
                        gameObjectForDifficulty2Or3.SetActive(true);
                        Debug.Log($"'{this.GetType().Name}': GameObject '{gameObjectForDifficulty2Or3.name}' ACTIVADO para dificultad {difficultyValue}.");
                    }
                    else
                    {
                        Debug.LogWarning($"'{this.GetType().Name}': 'GameObject For Difficulty 2 Or 3' no asignado, pero PlayerPrefs '{playerPrefsKeyDifficulty}' era {difficultyValue}.", this);
                    }
                }
                else
                {
                    Debug.Log($"'{this.GetType().Name}': El valor '{difficultyValue}' de la clave '{playerPrefsKeyDifficulty}' no corresponde a 1, 2 o 3. No se activó ningún GameObject por dificultad.");
                }

                if (clearDifficultyKeyAfterUse)
                {
                    PlayerPrefs.DeleteKey(playerPrefsKeyDifficulty);
                    PlayerPrefs.Save();
                    Debug.Log($"'{this.GetType().Name}': Clave '{playerPrefsKeyDifficulty}' eliminada.");
                }
            }
            else
            {
                Debug.Log($"'{this.GetType().Name}': No se encontró la clave '{playerPrefsKeyDifficulty}'. No se aplicará configuración de dificultad.");
                // Aquí podrías establecer una dificultad por defecto si lo deseas,
                // o simplemente dejar que la escena cargue como está configurada por defecto.
            }
        }
        else // cinematicConditionMet fue true
        {
            Debug.Log($"'{this.GetType().Name}': Cinemática final activada, se omite la configuración de dificultad.");
        }
    }

    void DisableGameObjects(List<GameObject> gameObjectsToDisable, string reason)
    {
        if (gameObjectsToDisable == null || gameObjectsToDisable.Count == 0)
        {
            return;
        }

        Debug.Log($"'{this.GetType().Name}': Desactivando {gameObjectsToDisable.Count} GameObjects para '{reason}'...");
        foreach (GameObject go in gameObjectsToDisable)
        {
            if (go != null)
            {
                go.SetActive(false);
                Debug.Log($"'{this.GetType().Name}': GameObject '{go.name}' desactivado.");
            }
        }
    }

    void OnValidate()
    {
        if (string.IsNullOrEmpty(playerPrefsKeyCinematic))
            Debug.LogWarning("La 'Player Prefs Key Cinematic' no debe estar vacía.", this);
        if (string.IsNullOrEmpty(requiredValueForCinematic))
            Debug.LogWarning("El 'Required Value For Cinematic' no debe estar vacío.", this);

        if (string.IsNullOrEmpty(playerPrefsKeyDifficulty))
            Debug.LogWarning("La 'Player Prefs Key Difficulty' no debe estar vacía.", this);

        WarnIfActive(gameObjectForFinalCinematic, "GameObject For Final Cinematic");
        WarnIfActive(gameObjectForDifficulty1, "GameObject For Difficulty 1");
        WarnIfActive(gameObjectForDifficulty2Or3, "GameObject For Difficulty 2 Or 3");

        // Validar si los PlayableDirectors están asignados si sus GameObjects contenedores lo están
        if (gameObjectForFinalCinematic != null && finalCinematicDirector == null)
        {
            // Podría ser que el director esté en el mismo GO, o en un hijo.
            // Si esperas que siempre esté, esta advertencia es útil.
            // Debug.LogWarning("'Final Cinematic Director' no está asignado pero su GameObject sí. ¿Es intencional?", this);
        }
    }

    void WarnIfActive(GameObject go, string fieldName)
    {
        if (go != null && go.activeSelf)
        {
            Debug.LogWarning($"'{go.name}' (asignado a '{fieldName}') debería estar INACTIVO por defecto en la escena para que este script lo active condicionalmente.", go);
        }
    }
}