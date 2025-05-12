using UnityEngine;
using UnityEngine.UI; // Para los componentes Button
// using UnityEngine.SceneManagement; // No es necesario si SceneTransitionManager lo maneja

public class GameStartMenu : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    [Tooltip("El índice de la escena a la que se transitará (en Build Settings).")]
    public int targetSceneIndex = 2; // Puedes cambiar esto si cada botón va a una escena diferente

    [Header("PlayerPrefs Key")]
    [Tooltip("La clave que se usará para guardar la opción seleccionada en PlayerPrefs.")]
    public string menuOptionKey = "SelectedMenuOption";

    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button aboutButton;
    public Button quitButton; // Aunque "Quit" normalmente cierra el juego, lo haremos transicionar

    void Start()
    {
        // Asegurarse de que los botones estén asignados
        if (startButton == null || aboutButton == null || quitButton == null)
        {
            Debug.LogError("Uno o más botones no están asignados en el GameStartMenu.");
            return;
        }

        // Hook events (asignar funciones a los clics de los botones)
        startButton.onClick.AddListener(HandleStartGame);
        aboutButton.onClick.AddListener(HandleAbout);
        quitButton.onClick.AddListener(HandleQuitGame); // Cambiado para transicionar
    }

    void HandleStartGame()
    {
        Debug.Log("Botón Start presionado.");
        PlayerPrefs.SetInt(menuOptionKey, 1); // 1 para "8-12"
        PlayerPrefs.Save(); // Es buena práctica guardar inmediatamente después de Set
        LoadTargetScene();
    }

    void HandleAbout()
    {
        Debug.Log("Botón About presionado.");
        PlayerPrefs.SetInt(menuOptionKey, 2); // 2 para "12-15"
        PlayerPrefs.Save();
        LoadTargetScene();
    }

    void HandleQuitGame()
    {
        Debug.Log("Botón Quit (transición) presionado.");
        PlayerPrefs.SetInt(menuOptionKey, 3); // 3 para "15-18" (interpretado como una opción de menú)
        PlayerPrefs.Save();
        LoadTargetScene();

        // Si realmente quisieras que el botón "Quit" cierre la aplicación en ciertas plataformas
        // y solo transicione en otras (o como fallback), podrías añadir lógica condicional:
        // #if UNITY_STANDALONE || UNITY_EDITOR
        //     Debug.Log("Cerrando aplicación (Quit).");
        //     Application.Quit();
        // #else
        //     // En WebGL u otras plataformas, Application.Quit() puede no funcionar o tener otro comportamiento.
        //     // Así que aquí es donde la transición de escena como fallback tiene sentido.
        //     PlayerPrefs.SetInt(menuOptionKey, 3);
        //     PlayerPrefs.Save();
        //     LoadTargetScene();
        // #endif
    }

    void LoadTargetScene()
    {
        // Ocultar el menú actual si es necesario
        // Si este script está en el Canvas principal del menú, podrías hacer:
        // gameObject.SetActive(false); // o canvas.enabled = false;
        // Esto es opcional y depende de cómo manejes la visibilidad del menú durante la transición.

        if (SceneTransitionManager.singleton == null)
        {
            Debug.LogError("SceneTransitionManager.singleton es NULO. No se puede cambiar de escena.");
            // Aquí podrías intentar cargar la escena de forma síncrona como fallback si es crítico,
            // o simplemente mostrar un error al usuario.
            // UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneIndex); // ¡CUIDADO! Esto es síncrono y congelará el juego.
            return;
        }
        SceneTransitionManager.singleton.GoToSceneAsync(targetSceneIndex);
    }

}