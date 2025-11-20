using UnityEngine;

public class RotateDoor : MonoBehaviour
{
    private Camera camara;

    void Start()
    {
        // Se guarda posición de la cámara
        camara = GetComponent<Camera>();
    }

    void Update()
    {
        //Para activar la selección, usamos el botón del centro del mouse.
        if (Input.GetMouseButtonDown(2))
        {
            //Se hace un rayo para detectar el modelo
            Ray rayo = camara.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(rayo, out hit))
            {
                if (hit.collider.CompareTag("lunarDoor"))
                {
                    Debug.Log("Puerta Detectada");
                    hit.transform.Rotate(0, 0, 90);
                }
            }
        }
    }
}
