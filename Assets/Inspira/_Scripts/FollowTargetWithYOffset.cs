using UnityEngine;

public class FollowTargetWithYOffset : MonoBehaviour
{
    [Tooltip("El Transform del GameObject cuya posici�n este objeto seguir�.")]
    public Transform targetToFollow;

    [Tooltip("El desfase vertical (en el eje Y) que se aplicar� a la posici�n del objetivo.")]
    public float yOffset = 1.0f; // Puedes ajustar este valor por defecto en el Inspector

    // Opcional: Si quieres que el seguimiento sea m�s suave, puedes usar estas variables
    // [Tooltip("Qu� tan suave ser� el seguimiento de posici�n. 0 = sin suavizado, valores m�s altos = m�s lento.")]
    // public float smoothSpeed = 0.125f;

    void LateUpdate() // Usar LateUpdate es generalmente mejor para scripts de seguimiento (como c�maras o este)
    {
        if (targetToFollow == null)
        {
            // Si no hay un objetivo asignado, no hacemos nada para evitar errores.
            // Podr�as a�adir un Debug.LogWarning aqu� si quieres ser notificado en la consola.
            // Debug.LogWarning("TargetToFollow no est� asignado en el objeto: " + gameObject.name);
            return;
        }

        // --- M�todo Directo (sin suavizado) ---
        // Obtener la posici�n del objetivo
        Vector3 targetPosition = targetToFollow.position;

        // Calcular la nueva posici�n para este GameObject
        // Mantenemos la X y Z del objetivo, pero ajustamos la Y con el desfase
        Vector3 newPosition = new Vector3(targetPosition.x,
                                          targetPosition.y + yOffset,
                                          targetPosition.z);

        // Asignar la nueva posici�n a este GameObject
        transform.position = newPosition;


        // --- M�todo con Suavizado (Opcional, descomenta si lo quieres) ---
        /*
        Vector3 desiredPosition = new Vector3(targetToFollow.position.x,
                                              targetToFollow.position.y + yOffset,
                                              targetToFollow.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime); // Time.deltaTime para que sea independiente del framerate
        transform.position = smoothedPosition;
        */

        // Opcional: Si tambi�n quieres que este objeto mire hacia el objetivo (o en la misma direcci�n)
        // transform.LookAt(targetToFollow); // Har� que mire directamente al centro del target
        // o
        // transform.rotation = targetToFollow.rotation; // Copiar� la rotaci�n exacta del target
    }

    // Opcional: Una funci�n OnValidate para dar feedback en el editor si no se ha asignado el target.
    void OnValidate()
    {
        if (targetToFollow == null)
        {
            // Esta advertencia solo aparecer� en el editor, no en una build.
            // Debug.LogWarning("No has asignado un 'Target To Follow' en el objeto: " + gameObject.name, this);
        }
    }
}