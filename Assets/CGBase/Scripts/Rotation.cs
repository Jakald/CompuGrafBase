using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float speedRotation = 100f;

    void Update()
    {
        transform.Rotate(Vector3.right, -speedRotation * Time.deltaTime);
    }
}
