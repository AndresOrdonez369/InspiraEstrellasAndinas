using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float lookSpeed = 2f;
    public float lookUpDownLimit = 80f;
    
    private CharacterController characterController;
    private Camera playerCamera;
    private float rotationX = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;  // Bloquear el cursor para que no se vea
        Cursor.visible = false;  // Ocultar el cursor
    }

    void Update()
    {
        // Movimiento del jugador
        float moveDirectionY = Input.GetAxis("Vertical") * walkSpeed;
        float moveDirectionX = Input.GetAxis("Horizontal") * walkSpeed;
        Vector3 moveDirection = transform.TransformDirection(moveDirectionX, 0, moveDirectionY);
        
        characterController.Move(moveDirection * Time.deltaTime);

        // Rotación de la cámara (mirada hacia arriba y hacia abajo)
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookUpDownLimit, lookUpDownLimit);  // Limitar la rotación vertical

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        
        // Rotación del personaje (mirada hacia los lados)
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * lookSpeed);
    }
}
