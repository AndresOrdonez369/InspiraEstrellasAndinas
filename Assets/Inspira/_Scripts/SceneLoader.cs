using UnityEngine;
using UnityEngine.SceneManagement; // �Necesario para manejar escenas!

public class SceneLoader : MonoBehaviour
{
    // Esta funci�n ser� llamada por la Se�al del Timeline
    public void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Cargando escena: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("�El nombre de la escena est� vac�o en el SceneLoader!");
        }
    }

    // Tambi�n puedes tener una funci�n para cargar por �ndice (opcional)
    // public void LoadSceneByIndex(int sceneIndex)
    // {
    //     SceneManager.LoadScene(sceneIndex);
    // }
}