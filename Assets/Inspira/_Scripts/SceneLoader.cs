using UnityEngine;
using UnityEngine.SceneManagement; // ¡Necesario para manejar escenas!

public class SceneLoader : MonoBehaviour
{
    // Esta función será llamada por la Señal del Timeline
    public void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Cargando escena: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("¡El nombre de la escena está vacío en el SceneLoader!");
        }
    }

    // También puedes tener una función para cargar por índice (opcional)
    // public void LoadSceneByIndex(int sceneIndex)
    // {
    //     SceneManager.LoadScene(sceneIndex);
    // }
}