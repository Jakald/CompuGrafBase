using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]  // Aseguramos que exista un animator
public class MonsterMove : MonoBehaviour
{

    [Header("Config. Movimiento")]
    public float walkVelocity = 2.0f;
    public float timeWalk = 3.0f;

    [Header("Giro del modelo")]
    public float spinVelocity = 180f;
    public float timeWait = 1f;

    [Header("Animacion")]
    public string nameWalk = "IsWalking";

    private Animator MyAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MyAnimator = GetComponent<Animator>();

        StartCoroutine(RoutinePatrol());
    }

    IEnumerator RoutinePatrol()
    {
        while(true) //Condicional para hacerlo de forma indefinida
        {
            //Activamos animación
            MyAnimator.SetBool(nameWalk, true);

            float cronometer = 0f;
            while(cronometer < timeWalk)
            {
                transform.Translate(Vector3.forward * walkVelocity * Time.deltaTime);

                cronometer += Time.deltaTime;

                yield return null;
            }
            //Detenemos animación para girar
            MyAnimator.SetBool(nameWalk, false);

            yield return new WaitForSeconds(timeWait);

            //Giramos al lado contrario
            float degreesSpin = 0f;
            Quaternion initialRotation = transform.rotation;

            while (degreesSpin < 180f)
            {
                float spinStep = spinVelocity * Time.deltaTime;
                transform.Rotate(0, spinStep, 0);
                degreesSpin += spinStep;
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
