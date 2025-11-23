using UnityEngine;

public class NaveMovement : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Cámara de referencia para el movimiento (si está vacío, usa la cámara principal)")]
    public Transform cameraReference;
    
    [Header("Configuración de Movimiento")]
    [Tooltip("Velocidad de movimiento de la nave")]
    public float moveSpeed = 5f;
    
    [Header("Animator")]
    [Tooltip("Animator de la nave (si está vacío, intenta obtenerlo del mismo GameObject)")]
    public Animator naveAnimator;
    
    private Vector3 moveDirection;
    private bool isMoving;

    void Start()
    {
        // Si no hay referencia de cámara, usar la cámara principal
        if (cameraReference == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                cameraReference = mainCam.transform;
            }
            else
            {
                Debug.LogWarning("NaveMovement: No se encontró cámara principal. El movimiento puede no funcionar correctamente.");
            }
        }
        
        // Si no hay referencia de Animator, intentar obtenerlo del mismo GameObject
        if (naveAnimator == null)
        {
            naveAnimator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (cameraReference == null) return;

        moveDirection = Vector3.zero;
        isMoving = false;

        // I = W (adelante) - dirección forward de la cámara
        if (Input.GetKey(KeyCode.I))
        {
            moveDirection += cameraReference.forward;
            isMoving = true;
        }

        // J = A (izquierda) - dirección negativa right de la cámara
        if (Input.GetKey(KeyCode.J))
        {
            moveDirection -= cameraReference.right;
            isMoving = true;
        }

        // K = S (atrás) - dirección negativa forward de la cámara
        if (Input.GetKey(KeyCode.K))
        {
            moveDirection -= cameraReference.forward;
            isMoving = true;
        }

        // L = D (derecha) - dirección right de la cámara
        if (Input.GetKey(KeyCode.L))
        {
            moveDirection += cameraReference.right;
            isMoving = true;
        }

        // Normalizar la dirección si hay múltiples teclas presionadas
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        // Aplicar movimiento
        if (isMoving)
        {
            // Mantener la componente Y en 0 para movimiento horizontal (o ajustar según necesites)
            moveDirection.y = 0;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
}

