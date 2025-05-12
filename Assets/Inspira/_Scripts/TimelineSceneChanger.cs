using UnityEngine;
using UnityEngine.Playables; // Necesario para PlayableDirector
using UnityEngine.SceneManagement; // Necesario para SceneManager

// Asegura que este script est� en un GameObject con un PlayableDirector
[RequireComponent(typeof(PlayableDirector))]
public class TimelineSceneChanger : MonoBehaviour
{
    [Header("Configuraci�n de Escena")]
    [Tooltip("El nombre EXACTO de la escena a la que quieres cambiar.")]
    public string targetSceneName;

    [Header("PlayerPrefs (Opcional)")]
    [Tooltip("La clave que se usar� para guardar la escena de origen.")]
    public string playerPrefsKey = "recuerdo"; // Puedes cambiar esta clave si quieres

    private PlayableDirector playableDirector;

    void Awake()
    {
        // Obtener la referencia al PlayableDirector en este mismo GameObject
        playableDirector = GetComponent<PlayableDirector>();

        // Comprobaci�n de errores inicial
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError($"TimelineSceneChanger: �El nombre de la escena destino ('Target Scene Name') no puede estar vac�o en el objeto {gameObject.name}!", this);
        }
    }

    // Suscribirse al evento cuando el objeto se activa
    void OnEnable()
    {
        if (playableDirector != null)
        {
            // El evento 'stopped' se dispara cuando la timeline termina o es detenida
            playableDirector.stopped += OnTimelineStopped;
        }
    }

    // Desuscribirse del evento cuando el objeto se desactiva para evitar errores
    void OnDisable()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnTimelineStopped;
        }
    }

    // Esta funci�n se llamar� cuando el PlayableDirector asociado se detenga (termine)
    private void OnTimelineStopped(PlayableDirector director)
    {
        // Doble comprobaci�n para asegurarnos de que es nuestro director el que se detuvo
        if (director == playableDirector)
        {
            Debug.Log($"Timeline '{director.playableAsset.name}' ha terminado. Preparando cambio de escena...");

            // 1. Guardar la escena actual en PlayerPrefs
            string currentSceneName = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString(playerPrefsKey, currentSceneName);
            PlayerPrefs.Save(); // Guarda los cambios inmediatamente
            Debug.Log($"PlayerPrefs: Guardado '{currentSceneName}' en la clave '{playerPrefsKey}'.");


            // 2. Cambiar a la escena destino (si el nombre no est� vac�o)
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                Debug.Log($"Cambiando a la escena: {targetSceneName}");
                SceneManager.LoadScene(targetSceneName);
            }
            else
            {
                Debug.LogError($"TimelineSceneChanger: Imposible cambiar de escena porque 'Target Scene Name' est� vac�o en {gameObject.name}", this);
            }
        }
    }
}