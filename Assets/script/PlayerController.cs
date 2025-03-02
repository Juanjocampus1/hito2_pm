using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float mouseSensitivity = 2.0f;
    public float jumpForce = 5.0f;
    private float verticalRotation = 0.0f;
    public float upDownRange = 60.0f;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    public float gravity = 20.0f;

    [Header("Shooting Settings")]
    public PrefabPool prefabPool; // Referencia a la pool de prefabs
    public float shootForce = 10f; // Fuerza inicial del disparo

    void Start()
    {
        // Bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Rotación del personaje
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0);

        // Rotación de la cámara
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // Movimiento del personaje
        if (characterController.isGrounded)
        {
            float currentSpeed = Input.GetKey(KeyCode.LeftControl) ? sprintSpeed : speed;
            float forwardMovement = Input.GetAxis("Vertical") * currentSpeed;
            float sideMovement = Input.GetAxis("Horizontal") * currentSpeed;

            moveDirection = transform.forward * forwardMovement + transform.right * sideMovement;

            // Salto del personaje
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }

        // Aplicar gravedad
        moveDirection.y -= gravity * Time.deltaTime;

        // Mover el personaje
        characterController.Move(moveDirection * Time.deltaTime);

        // Disparar esfera
        if (Input.GetButtonDown("Fire1"))
        {
            LaunchSphere();
        }
    }

    void LaunchSphere()
    {
        // Pedir una esfera a la pool
        GameObject sphere = prefabPool.GetPoolObject();
        if (sphere != null)
        {
            // Posicionar la esfera desactivada en la posición del jugador
            sphere.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
            sphere.transform.rotation = Camera.main.transform.rotation;

            // Activar la esfera
            sphere.SetActive(true);

            // Acceder al rigidbody de la esfera y aplicarle una fuerza hacia adelante
            Rigidbody rb = sphere.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Camera.main.transform.forward * shootForce;
            }
        }
        else
        {
            Debug.Log("No hay esferas disponibles en la pool");
        }

        Debug.Log("Lanzando esfera");
    }

    public void TakeDamage()
    {
        Die();
    }

    public void Die()
    {
        // Mostrar la UI de muerte
        FindObjectOfType<DeathUI>().ShowDeathUI();
        Debug.Log("Jugador ha muerto");
    }
}


