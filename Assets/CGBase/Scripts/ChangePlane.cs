using UnityEngine;

public class ChangePlane : MonoBehaviour
{

    [Header("Configuracion de Tecla")]
    public KeyCode keyToChange = KeyCode.C;  //Usamos la tecla C para cambiar la nave.

    [Header("Tu nave aqui")]
    public GameObject[] planeModels; //Usamos un array para guardar las naves.

    private int i = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateModel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToChange))
        {
            SwitchToNextPlane();
        }
    }

    void SwitchToNextPlane()
    {
        i++;

        if (i >= planeModels.Length)
        {
            i = 0;
        }

        UpdateModel();
    }

    void UpdateModel()
    {
        for (int j = 0; j < planeModels.Length; j++)
        {
            if ( j == i )
            {
                planeModels[j].SetActive(true);
            }
            else
            {
                planeModels[j].SetActive(false);
            }
        }
    }
}
