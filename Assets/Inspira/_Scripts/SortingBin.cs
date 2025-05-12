using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Asegura que haya un AudioSource
public class SortingBin : MonoBehaviour
{
    public string acceptedTag; // "Almacenable" o "Desecho" (configurar en Inspector)
    public SimpleGameManager gameManager; // Arrastra aquí GameLogicManager
    public AudioClip successSound; // Arrastra aquí el clip de audio para cuando se coloque correctamente

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError($"SortingBin en {name} no pudo encontrar su componente AudioSource.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(acceptedTag))
        {
            Debug.Log($"Objeto con tag '{acceptedTag}' entró en '{name}'. Destruyendo y reproduciendo sonido.");

            // 1. Reproducir el sonido
            if (audioSource != null && successSound != null)
            {
                // audioSource.PlayOneShot(successSound);
                // O si quieres controlar el volumen desde el script:
                audioSource.PlayOneShot(successSound, 1.0f); // El segundo parámetro es volumeScale
            }
            else
            {
                if (audioSource == null) Debug.LogWarning($"AudioSource es nulo en {name}. No se puede reproducir sonido.");
                if (successSound == null) Debug.LogWarning($"SuccessSound no asignado en {name}. No se puede reproducir sonido.");
            }

            // 2. Notificar al GameManager ANTES de destruir, por si el GameManager necesita info del objeto
            if (gameManager != null)
            {
                gameManager.RockSorted(acceptedTag);
            }
            else
            {
                Debug.LogWarning($"GameManager no asignado en {name}.");
            }

            // 3. Destruir el objeto
            // Es importante que el sonido se inicie ANTES de destruir el objeto que colisionó,
            // especialmente si el AudioSource estuviera en el objeto 'other' (que no es el caso aquí).
            Destroy(other.gameObject);
        }
    }
}