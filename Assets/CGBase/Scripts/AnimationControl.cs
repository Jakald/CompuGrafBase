using UnityEngine;

[RequireComponent (typeof(Animator))]
public class AnimationControl : MonoBehaviour
{
    private Animator myAnimator;
    private bool actualState = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Busca en el objeto el componente de animación
        myAnimator = GetComponent<Animator>();
    }

    public void Interact()
    {
        //Acutualiza el estado de la animación para activarla, si ya se hizo, realiza el cierre.
        actualState = !actualState;

        myAnimator.SetBool("Abierto", actualState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
