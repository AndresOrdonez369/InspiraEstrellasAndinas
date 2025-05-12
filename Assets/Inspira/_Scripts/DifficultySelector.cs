using UnityEngine;
using UnityEngine.UI; // Aún necesario si interactúas con UI
using UnityEngine.SceneManagement; // ¡Ahora es crucial!
using UnityEngine.Serialization; // Útil si renombras variables serializadas

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
        SetDifficultyAndLoadScene(1, "Fácil");
    }

    public void SelectDifficultyMedium()
    {
        SetDifficultyAndLoadScene(2, "Media");
    }

    public void SelectDifficultyHard()
    {
        SetDifficultyAndLoadScene(3, "Difícil");
    }

    // --- Función combinada para guardar dificultad y cargar escena ---
    private void SetDifficultyAndLoadScene(int difficultyLevel, string difficultyName)
    {
        // 1. Guarda la dificultad
        PlayerPrefs.SetInt(DifficultyKey, difficultyLevel);
        PlayerPrefs.Save(); // Asegura que se guarde antes de cambiar de escena
        Debug.Log($"Dificultad seleccionada y guardada: {difficultyName} ({difficultyLevel})");

        // 2. Carga la escena especificada
        LoadGameScene();
    }

    // --- Función para cargar la escena del juego ---
    private void LoadGameScene()
    {
        // Comprobación de seguridad: ¿Se asignó un nombre de escena en el Inspector?
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("¡ERROR! No se ha especificado el nombre de la escena a cargar en el Inspector del DifficultySelector.");
            return; 
        }

        // Asegúrate de que el nombre proporcionado coincida EXACTAMENTE
        Debug.Log($"Cargando la escena: {gameSceneName}...");
        SceneManager.LoadScene(gameSceneName);
    }
}