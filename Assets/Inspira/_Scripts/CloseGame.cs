using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CloseGame : MonoBehaviour
{
    // Asigna este m�todo al evento "OnClick" del bot�n en el Inspector
    public void QuitGame()
    {
        // Cierra la aplicaci�n
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
}
