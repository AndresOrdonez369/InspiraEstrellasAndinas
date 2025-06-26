using UnityEditor;
using UnityEngine;

public class PlayerPrefsEditor
{
    [MenuItem("Tools/Clear PlayerPrefs (All)")]
    private static void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All PlayerPrefs have been deleted.");
    }

    [MenuItem("Tools/Clear Specific Prefs (Recuerdo/Cinematic)")]
    private static void ClearMySpecificPrefs()
    {
        // Usa las claves exactas que estás usando en tus scripts
        string cinematicKey = "recuerdo"; // O "activarCinematicaFinal" si es esa la que usas en el Inspector
        string difficultyKey = "dificultadNave";

        if (PlayerPrefs.HasKey(cinematicKey))
        {
            PlayerPrefs.DeleteKey(cinematicKey);
            Debug.Log($"PlayerPref '{cinematicKey}' deleted.");
        }
        else
        {
            Debug.Log($"PlayerPref '{cinematicKey}' not found, nothing to delete.");
        }

        if (PlayerPrefs.HasKey(difficultyKey))
        {
            PlayerPrefs.DeleteKey(difficultyKey);
            Debug.Log($"PlayerPref '{difficultyKey}' deleted.");
        }
        else
        {
            Debug.Log($"PlayerPref '{difficultyKey}' not found, nothing to delete.");
        }
        PlayerPrefs.Save();
    }
}