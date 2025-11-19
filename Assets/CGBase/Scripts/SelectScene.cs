using UnityEngine.SceneManagement;
using UnityEngine;

public class SelectScene : MonoBehaviour
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
                //Detecta si colisiona con un objeto
                TargetScene target = hit.collider.GetComponent<TargetScene>();

                //Si no es nulo, intenta cargar la escena.
                if (target != null) {
                    string invoque = target.sceneName;

                    if (!string.IsNullOrEmpty(invoque))
                    {
                        Debug.Log("Cargando Escena" + invoque);
                        SceneManager.LoadScene(invoque);
                    }
                }
            }
        }
    }
}
