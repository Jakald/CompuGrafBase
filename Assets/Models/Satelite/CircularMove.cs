using UnityEngine;

public class CircularMove : MonoBehaviour
{
    [Header("Órbita")]
    public Transform center;
    public float orbitSpeed = 20f;

    [Header("Rotación propia")]
    public float spinSpeed = 60f;

    void Start()
    {
        if (center == null)
        {
            GameObject go = new GameObject("OrbitCenter");
            go.transform.position = Vector3.zero;   // o transform.position si prefieres
            center = go.transform;
        }
    }

    void Update()
    {
        // Órbita
        transform.RotateAround(center.position, Vector3.up, orbitSpeed * Time.deltaTime);

        // Giro del satélite
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
    }
}
