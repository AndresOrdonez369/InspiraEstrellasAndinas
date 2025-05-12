using UnityEngine;
using UnityEngine.UI; // A�n necesario si interact�as con UI
using UnityEngine.SceneManagement; // �Ahora es crucial!
using UnityEngine.Serialization; // �til si renombras variables serializadas

public class DifficultySelector : MonoBehaviour
{
    // Clave para PlayerPrefs
    public const string DifficultyKey = "SelectedDifficulty";

    [Header("Scene Loading")]
    [SerializeField] 
    [Tooltip("El nombre EXACTO de la escena del juego a cargar (debe estar en Build Settings).")]
    private string gameSceneName = "TuEscenaDeJuego"; // Nombre de la escena a cargar, editable en Inspector


    public void SelectDifficultyEasy()
    {
        SetDifficultyAndLoadScene(1, "F�cil");
    }

    public void SelectDifficultyMedium()
    {
        SetDifficultyAndLoadScene(2, "Media");
    }

    public void SelectDifficultyHard()
    {
        SetDifficultyAndLoadScene(3, "Dif�cil");
    }

    // --- Funci�n combinada para guardar dificultad y cargar escena ---
    private void SetDifficultyAndLoadScene(int difficultyLevel, string difficultyName)
    {
        // 1. Guarda la dificultad
        PlayerPrefs.SetInt(DifficultyKey, difficultyLevel);
        PlayerPrefs.Save(); // Asegura que se guarde antes de cambiar de escena
        Debug.Log($"Dificultad seleccionada y guardada: {difficultyName} ({difficultyLevel})");

        // 2. Carga la escena especificada
        LoadGameScene();
    }

    // --- Funci�n para cargar la escena del juego ---
    private void LoadGameScene()
    {
        // Comprobaci�n de seguridad: �Se asign� un nombre de escena en el Inspector?
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("�ERROR! No se ha especificado el nombre de la escena a cargar en el Inspector del DifficultySelector.");
            return; 
        }

        // Aseg�rate de que el nombre proporcionado coincida EXACTAMENTE
        Debug.Log($"Cargando la escena: {gameSceneName}...");
        SceneManager.LoadScene(gameSceneName);
    }
}