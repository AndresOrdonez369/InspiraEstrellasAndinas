using UnityEngine;

public class PiezaConSocketHijo : MonoBehaviour
{
    // ¡IMPORTANTE! Arrastra el GameObject del Socket Hijo aquí desde la Jerarquía/Prefab
    public GameObject socketHijo;

    // Esta función será llamada por el evento del Socket Padre cuando esta pieza entre
    public void ActivarSocketHijo()
    {
        if (socketHijo != null)
        {
            socketHijo.SetActive(true);
            Debug.Log($"Socket Hijo '{socketHijo.name}' ACTIVADO en: {gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"Se intentó activar un Socket Hijo nulo en: {gameObject.name}");
        }
    }

    // Esta función será llamada por el evento del Socket Padre cuando esta pieza salga
    public void DesactivarSocketHijo()
    {
        if (socketHijo != null)
        {
            // Considera si realmente quieres desactivarlo al salir.
            // A veces es mejor dejarlo activo si otra pieza puede entrar.
            // Si solo puede haber una pila, desactivarlo está bien.
            socketHijo.SetActive(false);
            Debug.Log($"Socket Hijo '{socketHijo.name}' DESACTIVADO en: {gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"Se intentó desactivar un Socket Hijo nulo en: {gameObject.name}");
        }
    }
}