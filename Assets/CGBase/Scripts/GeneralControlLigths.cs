using UnityEngine;
using System.Collections;

public class GeneralControlLigths : MonoBehaviour
{
    private Light myLigth;

    [Header("Ritmo de Luz")]
    public float intervalDur = 1.0f;

    [Header("Estado de la luz")]
    public bool startLigth = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myLigth = GetComponent<Light>();

        if(myLigth == null )
        {
            Debug.LogError("No existe luz en "+ gameObject.name);
            return;
        }

        // Iniciamos Corutina
        StartCoroutine(CicloIntercalado());
    }

    IEnumerator CicloIntercalado()
    {
        myLigth.enabled = startLigth;

        while (true)
        {
            //Esperamos un momento para apagar las luces.
            yield return new WaitForSeconds(intervalDur);

            //invertimos el estado
            myLigth.enabled = !myLigth.enabled;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
