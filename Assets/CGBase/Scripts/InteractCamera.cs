using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Camera camara;
    public int wichButton = 0;


    void Start()
    {
        camara = GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(wichButton))
        {
            Ray rayo = camara.ScreenPointToRay(Input.mousePosition);
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
}
