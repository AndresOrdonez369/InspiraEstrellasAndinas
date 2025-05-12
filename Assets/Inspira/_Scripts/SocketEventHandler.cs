using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Necesario para los EventArgs

public class SocketEventHandler : MonoBehaviour
{
    // Función pública que llamaremos DESDE el evento del Inspector
    public void HandleSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log($"Evento Select Entered recibido por {gameObject.name}. Interactable: {args.interactableObject.transform.name}");

        // Intentamos obtener el script de la pieza que entró
        PiezaConSocketHijo pieza = args.interactableObject.transform.GetComponent<PiezaConSocketHijo>();

        if (pieza != null)
        {
            Debug.Log($"PiezaConSocketHijo encontrado en {args.interactableObject.transform.name}. Llamando a ActivarSocketHijo...");
            pieza.ActivarSocketHijo();
        }
        else
        {
            Debug.LogWarning($"El objeto {args.interactableObject.transform.name} no tiene el script PiezaConSocketHijo.");
        }
    }

    // Función para desactivar (similar)
    public void HandleSelectExited(SelectExitEventArgs args)
    {
        Debug.Log($"Evento Select Exited recibido por {gameObject.name}. Interactable: {args.interactableObject.transform.name}");

        PiezaConSocketHijo pieza = args.interactableObject.transform.GetComponent<PiezaConSocketHijo>();
        if (pieza != null)
        {
            Debug.Log($"PiezaConSocketHijo encontrado en {args.interactableObject.transform.name}. Llamando a DesactivarSocketHijo...");
            pieza.DesactivarSocketHijo();
        }
        else
        {
            Debug.LogWarning($"El objeto {args.interactableObject.transform.name} no tiene el script PiezaConSocketHijo.");
        }
    }
}