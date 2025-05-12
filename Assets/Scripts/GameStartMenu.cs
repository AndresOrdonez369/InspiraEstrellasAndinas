using UnityEngine;
using UnityEngine.UI; // Para los componentes Button
// using UnityEngine.SceneManagement; // No es necesario si SceneTransitionManager lo maneja

public class GameStartMenu : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    [Tooltip("El �ndice de la escena a la que se transitar� (en Build Settings).")]
    public int targetSceneIndex = 2; // Puedes cambiar esto si cada bot�n va a una escena diferente

    [Header("PlayerPrefs Key")]
    [Tooltip("La clave que se usar� para guardar la opci�n seleccionada en PlayerPrefs.")]
    public string menuOptionKey = "SelectedMenuOption";

    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button aboutButton;
    public Button quitButton; // Aunque "Quit" normalmente cierra el juego, lo haremos transicionar

    void Start()
    {
        // Asegurarse de que los botones est�n asignados
        if (startButton == null || aboutButton == null || quitButton == null)
        {
            Debug.LogError("Uno o m�s botones no est�n asignados en el GameStartMenu.");
            return;
        }

        // Hook events (asignar funciones a los clics de los botones)
        startButton.onClick.AddListener(HandleStartGame);
        aboutButton.onClick.AddListener(HandleAbout);
        quitButton.onClick.AddListener(HandleQuitGame); // Cambiado para transicionar
    }

    void HandleStartGame()
    {
        Debug.Log("Bot�n Start presionado.");
        PlayerPrefs.SetInt(menuOptionKey, 1); // 1 para "8-12"
        PlayerPrefs.Save(); // Es buena pr�ctica guardar inmediatamente despu�s de Set
        LoadTargetScene();
    }

    void HandleAbout()
    {
        Debug.Log("Bot�n About presionado.");
        PlayerPrefs.SetInt(menuOptionKey, 2); // 2 para "12-15"
        PlayerPrefs.Save();
        LoadTargetScene();
    }

    void HandleQuitGame()
    {
        Debug.Log("Bot�n Quit (transici�n) presionado.");
        PlayerPrefs.SetInt(menuOptionKey, 3); // 3 para "15-18" (interpretado como una opci�n de men�)
        PlayerPrefs.Save();
        LoadTargetScene();

        // Si realmente quisieras que el bot�n "Quit" cierre la aplicaci�n en ciertas plataformas
        // y solo transicione en otras (o como fallback), podr�as a�adir l�gica condicional:
        // #if UNITY_STANDALONE || UNITY_EDITOR
        //     Debug.Log("Cerrando aplicaci�n (Quit).");
        //     Application.Quit();
        // #else
        //     // En WebGL u otras plataformas, Application.Quit() puede no funcionar o tener otro comportamiento.
        //     // As� que aqu� es donde la transici�n de escena como fallback tiene sentido.
        //     PlayerPrefs.SetInt(menuOptionKey, 3);
        //     PlayerPrefs.Save();
        //     LoadTargetScene();
        // #endif
    }

    void LoadTargetScene()
    {
        // Ocultar el men� actual si es necesario
        // Si este script est� en el Canvas principal del men�, podr�as hacer:
        // gameObject.SetActive(false); // o canvas.enabled = false;
        // Esto es opcional y depende de c�mo manejes la visibilidad del men� durante la transici�n.

        if (SceneTransitionManager.singleton == null)
        {
            Debug.LogError("SceneTransitionManager.singleton es NULO. No se puede cambiar de escena.");
            // Aqu� podr�as intentar cargar la escena de forma s�ncrona como fallback si es cr�tico,
            // o simplemente mostrar un error al usuario.
            // UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneIndex); // �CUIDADO! Esto es s�ncrono y congelar� el juego.
            return;
        }
        SceneTransitionManager.singleton.GoToSceneAsync(targetSceneIndex);
    }

}