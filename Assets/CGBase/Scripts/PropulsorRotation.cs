using UnityEngine;

public enum EjeRotacion
{
    X,
    Y,
    Z
}

public class PropulsorRotation : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    [Tooltip("Velocidad de rotación de los propulsores en grados por segundo")]
    public float velocidadRotacion = 360f;
    
    [Header("Referencias de Propulsores")]
    [Tooltip("Referencias a los propulsores (se buscarán automáticamente si están vacías)")]
    public Transform propulsor1;
    [Tooltip("Eje de rotación para propulsor1")]
    public EjeRotacion ejePropulsor1 = EjeRotacion.Z;
    
    public Transform propulsor2;
    [Tooltip("Eje de rotación para propulsor2")]
    public EjeRotacion ejePropulsor2 = EjeRotacion.Z;
    
    public Transform propulsor3;
    [Tooltip("Eje de rotación para propulsor3")]
    public EjeRotacion ejePropulsor3 = EjeRotacion.Z;
    
    private void Start()
    {
        // Buscar los propulsores automáticamente si no están asignados
        if (propulsor1 == null)
        {
            propulsor1 = transform.Find("propulsor1");
            if (propulsor1 == null)
            {
                Debug.LogWarning($"PropulsorRotation: No se encontró 'propulsor1' en {gameObject.name}");
            }
        }
        
        if (propulsor2 == null)
        {
            propulsor2 = transform.Find("propulsor2");
            if (propulsor2 == null)
            {
                Debug.LogWarning($"PropulsorRotation: No se encontró 'propulsor2' en {gameObject.name}");
            }
        }
        
        if (propulsor3 == null)
        {
            propulsor3 = transform.Find("propulsor3");
            if (propulsor3 == null)
            {
                Debug.LogWarning($"PropulsorRotation: No se encontró 'propulsor3' en {gameObject.name}");
            }
        }
        
        // Verificar que al menos un propulsor fue encontrado
        if (propulsor1 == null && propulsor2 == null && propulsor3 == null)
        {
            Debug.LogError($"PropulsorRotation: No se encontraron propulsores en {gameObject.name}. " +
                          "Asegúrate de que los propulsores sean hijos directos y tengan los nombres correctos.");
        }
    }
    
    private void Update()
    {
        // Rotar cada propulsor sobre su eje configurado
        if (propulsor1 != null)
        {
            RotarPropulsor(propulsor1, ejePropulsor1);
        }
        
        if (propulsor2 != null)
        {
            RotarPropulsor(propulsor2, ejePropulsor2);
        }
        
        if (propulsor3 != null)
        {
            RotarPropulsor(propulsor3, ejePropulsor3);
        }
    }
    
    /// <summary>
    /// Rota un propulsor sobre el eje especificado
    /// </summary>
    private void RotarPropulsor(Transform propulsor, EjeRotacion eje)
    {
        Vector3 ejeRotacion = ObtenerVectorEje(eje);
        propulsor.Rotate(ejeRotacion * velocidadRotacion * Time.deltaTime, Space.Self);
    }
    
    /// <summary>
    /// Convierte el enum EjeRotacion a un Vector3 para usar en Rotate
    /// </summary>
    private Vector3 ObtenerVectorEje(EjeRotacion eje)
    {
        switch (eje)
        {
            case EjeRotacion.X:
                return Vector3.right;
            case EjeRotacion.Y:
                return Vector3.up;
            case EjeRotacion.Z:
                return Vector3.forward;
            default:
                return Vector3.forward;
        }
    }
}

