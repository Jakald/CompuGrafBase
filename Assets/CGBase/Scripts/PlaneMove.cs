using UnityEngine;

public class PlaneMove : MonoBehaviour
{
    [Header("Velocidad de Movimiento")]
    public float velocidadAvance = 20f;

    [Header("Velocidad de Giro")]
    public float sensibilidadMouse = 100f; // Para apuntar (Mouse)
    public float velocidadRoll = 150f;     // Para rotar sobre sí misma (Teclas A/D)

    [Header("Ajustes")]
    public bool invertirMouseY = true;     // true = "Estilo Avión" (Mouse abajo sube la nariz)

    void Start()
    {
        // Ocultamos el cursor y lo bloqueamos al centro
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- 1. LEER INPUTS ---

        // Mouse (Para apuntar la nariz)
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse * Time.deltaTime;

        // Teclado A/D (Para rotar sobre sí mismo - Roll)
        // Input.GetAxis("Horizontal") devuelve: -1 con la A (Izquierda), 1 con la D (Derecha)
        float inputRoll = Input.GetAxis("Horizontal") * velocidadRoll * Time.deltaTime;

        // Teclado W/S (Para Acelerar/Frenar)
        float inputAceleracion = Input.GetAxis("Vertical");


        // --- 2. APLICAR ROTACIONES ---

        // A. PITCH (Morro Arriba/Abajo con Mouse Y)
        // Si invertimos Y, rotamos en positivo, si no, en negativo
        Vector3 rotacionPitch = Vector3.right * (invertirMouseY ? mouseY : -mouseY);
        transform.Rotate(rotacionPitch);

        // B. YAW (Girar Izq/Der con Mouse X)
        transform.Rotate(Vector3.up * mouseX);

        // C. ROLL (Rotar sobre sí misma con A/D)
        // Usamos Vector3.back para que la 'A' rote a la izquierda y 'D' a la derecha
        transform.Rotate(Vector3.back * inputRoll);


        // --- 3. APLICAR MOVIMIENTO (Propulsión) ---

        // Siempre avanzamos hacia donde mira la nariz (forward)
        Vector3 propulsion = transform.forward * inputAceleracion * velocidadAvance * Time.deltaTime;
        transform.position += propulsion;

        // Opcional: Estabilizar el eje Z (Roll) automáticamente si sueltas las teclas
        // (Por ahora desactivado para que tengas libertad total de giro)
    }

    // Desbloquear mouse con Escape por si necesitas salir
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}