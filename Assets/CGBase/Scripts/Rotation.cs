using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float speedRotation = 100f;

    public Vector3 rotationAxis = new Vector3(1, 0, 0);

    void Update()
    {
        transform.Rotate(rotationAxis, -speedRotation * Time.deltaTime);
    }
}
