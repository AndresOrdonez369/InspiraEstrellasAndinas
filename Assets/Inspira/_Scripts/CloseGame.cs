using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CloseGame : MonoBehaviour
{
    // Asigna este método al evento "OnClick" del botón en el Inspector
    public void QuitGame()
    {
        // Cierra la aplicación
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
}
