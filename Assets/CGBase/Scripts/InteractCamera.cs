using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Camera camaraActual;
    public int wichButton = 0;


    void Start()
    {
        camaraActual = GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(wichButton))
        {
            Ray rayo = camaraActual.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(rayo,out hit))
            {
                // ¿Se cambia de escena?
                TargetScene target = hit.collider.GetComponent<TargetScene>();
                if (target != null)
                {
                    string invoque = target.sceneName;
                    if (!string.IsNullOrEmpty(invoque))
                    {
                        Debug.Log("Cargando Escena" + invoque);
                        SceneManager.LoadScene(invoque);
                    }
                }

                // ¿Se activa una animación?

                AnimationControl objetoAnimado = hit.collider.GetComponent<AnimationControl>();
                if (objetoAnimado != null)
                {
                    Debug.Log("Activando Animación");
                    objetoAnimado.Interact();
                }

                //¿Se quiere cambiar de cámara?

                goToPlaneCamera cameraDetected = hit.collider.GetComponent<goToPlaneCamera>();

                if (cameraDetected != null)
                {
                    Debug.Log("Cambiando a Camara de nave");
                    changeCamera(cameraDetected.planeCamera);
                    return;
                }

                // ¿Se quiere salir de la app?

                if (hit.collider.CompareTag("quitButton"))
                {
                    Debug.Log("Cerrando Aplicación");
                    Application.Quit();
                    return;
                }
            }
        }
    }

    void changeCamera(Camera newCamera)
    {
        if (newCamera != null)
        {
            //"Enciende la camara de la nave"
            newCamera.gameObject.SetActive(true);

            //Activamos el Script del movimiento de la nave
            PlaneMove moving = newCamera.GetComponentInParent<PlaneMove>();

            if (moving != null)
            {
                moving.enabled = true;
                Debug.Log("Se puede mover la nave");
            }
            else
            {
                Debug.LogError("No se encontró Script");
            }

                //Apagamos la camara principal
                gameObject.SetActive(false);
        }

    }
}
