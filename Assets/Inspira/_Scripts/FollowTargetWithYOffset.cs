using UnityEngine;

public class FollowTargetWithYOffset : MonoBehaviour
{
    [Tooltip("El Transform del GameObject cuya posición este objeto seguirá.")]
    public Transform targetToFollow;

    [Tooltip("El desfase vertical (en el eje Y) que se aplicará a la posición del objetivo.")]
    public float yOffset = 1.0f; // Puedes ajustar este valor por defecto en el Inspector

    // Opcional: Si quieres que el seguimiento sea más suave, puedes usar estas variables
    // [Tooltip("Qué tan suave será el seguimiento de posición. 0 = sin suavizado, valores más altos = más lento.")]
    // public float smoothSpeed = 0.125f;

    void LateUpdate() // Usar LateUpdate es generalmente mejor para scripts de seguimiento (como cámaras o este)
    {
        if (targetToFollow == null)
        {
            // Si no hay un objetivo asignado, no hacemos nada para evitar errores.
            // Podrías añadir un Debug.LogWarning aquí si quieres ser notificado en la consola.
            // Debug.LogWarning("TargetToFollow no está asignado en el objeto: " + gameObject.name);
            return;
        }

        // --- Método Directo (sin suavizado) ---
        // Obtener la posición del objetivo
        Vector3 targetPosition = targetToFollow.position;

        // Calcular la nueva posición para este GameObject
        // Mantenemos la X y Z del objetivo, pero ajustamos la Y con el desfase
        Vector3 newPosition = new Vector3(targetPosition.x,
                                          targetPosition.y + yOffset,
                                          targetPosition.z);

        // Asignar la nueva posición a este GameObject
        transform.position = newPosition;


        // --- Método con Suavizado (Opcional, descomenta si lo quieres) ---
        /*
        Vector3 desiredPosition = new Vector3(targetToFollow.position.x,
                                              targetToFollow.position.y + yOffset,
                                              targetToFollow.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime); // Time.deltaTime para que sea independiente del framerate
        transform.position = smoothedPosition;
        */

        // Opcional: Si también quieres que este objeto mire hacia el objetivo (o en la misma dirección)
        // transform.LookAt(targetToFollow); // Hará que mire directamente al centro del target
        // o
        // transform.rotation = targetToFollow.rotation; // Copiará la rotación exacta del target
    }

    // Opcional: Una función OnValidate para dar feedback en el editor si no se ha asignado el target.
    void OnValidate()
    {
        if (targetToFollow == null)
        {
            // Esta advertencia solo aparecerá en el editor, no en una build.
            // Debug.LogWarning("No has asignado un 'Target To Follow' en el objeto: " + gameObject.name, this);
        }
    }
}