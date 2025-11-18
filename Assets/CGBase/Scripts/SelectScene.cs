using UnityEngine.SceneManagement;
using UnityEngine;

public class SelectScene : MonoBehaviour
{
public string SceneToLoad;

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
                //Si se detecta, carga la escena
            if (hit.collider.CompareTag("opCenter"))
            {
                Debug.Log("Cargando Escena: " + SceneToLoad);
                SceneManager.LoadScene(SceneToLoad);
            }
        }
    }
}
}
