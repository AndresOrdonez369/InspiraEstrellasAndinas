using UnityEngine;

public class PiezaConSocketHijo : MonoBehaviour
{
    // �IMPORTANTE! Arrastra el GameObject del Socket Hijo aqu� desde la Jerarqu�a/Prefab
    public GameObject socketHijo;

    // Esta funci�n ser� llamada por el evento del Socket Padre cuando esta pieza entre
    public void ActivarSocketHijo()
    {
        if (socketHijo != null)
        {
            socketHijo.SetActive(true);
            Debug.Log($"Socket Hijo '{socketHijo.name}' ACTIVADO en: {gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"Se intent� activar un Socket Hijo nulo en: {gameObject.name}");
        }
    }

    // Esta funci�n ser� llamada por el evento del Socket Padre cuando esta pieza salga
    public void DesactivarSocketHijo()
    {
        if (socketHijo != null)
        {
            // Considera si realmente quieres desactivarlo al salir.
            // A veces es mejor dejarlo activo si otra pieza puede entrar.
            // Si solo puede haber una pila, desactivarlo est� bien.
            socketHijo.SetActive(false);
            Debug.Log($"Socket Hijo '{socketHijo.name}' DESACTIVADO en: {gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"Se intent� desactivar un Socket Hijo nulo en: {gameObject.name}");
        }
    }
}