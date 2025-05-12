using UnityEngine;

public class PlayerPrefsChoiceActivator : MonoBehaviour
{
    [Header("PlayerPrefs Configuration")]
    [Tooltip("La clave de PlayerPrefs (tipo entero) que se leerá.")]
    public string playerPrefsKey = "miEleccionNumerica"; // Cambia esto a la clave que estés usando

    [Header("GameObjects a Activar")]
    [Tooltip("GameObject que se activará si el valor de PlayerPrefs es 1. Debe estar INACTIVO en la escena por defecto.")]
    public GameObject gameObjectForValue1;

    [Tooltip("GameObject que se activará si el valor de PlayerPrefs es 2 o 3. Debe estar INACTIVO en la escena por defecto.")]
    public GameObject gameObjectForValue2Or3;

    [Header("Opcional")]
    [Tooltip("Si se marca, la clave de PlayerPrefs se eliminará después de su uso.")]
    public bool clearPlayerPrefsAfterUse = true;

    void Awake()
    {
        // 1. Asegurarse de que los GameObjects estén desactivados inicialmente
        //    Esto es una salvaguarda si por error los dejaste activos en el editor.
        if (gameObjectForValue1 != null)
        {
            gameObjectForValue1.SetActive(false);
        }
        if (gameObjectForValue2Or3 != null)
        {
            gameObjectForValue2Or3.SetActive(false);
        }

        // 2. Comprobar la clave de PlayerPrefs
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            int choiceValue = PlayerPrefs.GetInt(playerPrefsKey);
            Debug.Log($"'{this.GetType().Name}': Se encontró la clave '{playerPrefsKey}' con valor '{choiceValue}'.");

            // 3. Activar el GameObject correspondiente
            if (choiceValue == 1)
            {
                if (gameObjectForValue1 != null)
                {
                    gameObjectForValue1.SetActive(true);
                    Debug.Log($"'{this.GetType().Name}': GameObject '{gameObjectForValue1.name}' ACTIVADO para valor 1.");
                }
                else
                {
                    Debug.LogWarning($"'{this.GetType().Name}': 'GameObject For Value 1' no está asignado, pero PlayerPrefs '{playerPrefsKey}' era 1.", this);
                }
            }
            else if (choiceValue == 2 || choiceValue == 3)
            {
                if (gameObjectForValue2Or3 != null)
                {
                    gameObjectForValue2Or3.SetActive(true);
                    Debug.Log($"'{this.GetType().Name}': GameObject '{gameObjectForValue2Or3.name}' ACTIVADO para valor {choiceValue}.");
                }
                else
                {
                    Debug.LogWarning($"'{this.GetType().Name}': 'GameObject For Value 2 Or 3' no está asignado, pero PlayerPrefs '{playerPrefsKey}' era {choiceValue}.", this);
                }
            }
            else
            {
                Debug.Log($"'{this.GetType().Name}': El valor '{choiceValue}' de la clave '{playerPrefsKey}' no corresponde a 1, 2 o 3. No se activó ningún GameObject por esta condición.");
            }

            // 4. Opcional: Limpiar la clave de PlayerPrefs
            if (clearPlayerPrefsAfterUse)
            {
                PlayerPrefs.DeleteKey(playerPrefsKey);
                PlayerPrefs.Save(); // Es buena práctica llamar a Save() después de DeleteKey o Set
                Debug.Log($"'{this.GetType().Name}': Clave '{playerPrefsKey}' eliminada de PlayerPrefs.");
            }
        }
        else
        {
            Debug.Log($"'{this.GetType().Name}': No se encontró la clave '{playerPrefsKey}'. No se activará ningún GameObject por esta condición.");
        }
    }

    // Opcional: Validación en el editor para ayudar a la configuración
    void OnValidate()
    {
        if (string.IsNullOrEmpty(playerPrefsKey))
        {
            Debug.LogWarning("La 'Player Prefs Key' no debe estar vacía.", this);
        }
        if (gameObjectForValue1 != null && gameObjectForValue1.activeSelf)
        {
            Debug.LogWarning($"'{gameObjectForValue1.name}' (asignado a 'GameObject For Value 1') debería estar INACTIVO por defecto en la escena para que este script lo active condicionalmente.", gameObjectForValue1);
        }
        if (gameObjectForValue2Or3 != null && gameObjectForValue2Or3.activeSelf)
        {
            Debug.LogWarning($"'{gameObjectForValue2Or3.name}' (asignado a 'GameObject For Value 2 Or 3') debería estar INACTIVO por defecto en la escena para que este script lo active condicionalmente.", gameObjectForValue2Or3);
        }
    }
}