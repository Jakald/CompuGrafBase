using UnityEngine;
using System.Collections;

public class MonsterWaveAttack : MonoBehaviour
{
    [Header("Punto de Origen de la Onda")]
    public Vector3 waveOriginOffset = Vector3.zero;
    
    [Header("Configuración de Tiempo")]
    public float waveInterval = 5f;
    public float initialDelay = 2f;
    
    [Header("Configuración de la Onda")]
    public float waveDistance = 100f;
    public float chargeDuration = 1f;
    public float chargeSpeed = 10f;
    public float launchSpeed = 100f;
    public float waveDuration = 3f;
    
    [Header("Apariencia de la Onda")]
    public Color waveColor = new Color(0f, 0.5f, 1f, 1f);
    public float initialSphereSize = 2f;
    public float finalSphereSize = 0.5f;
    public float maxLightIntensity = 10f;
    
    [Header("Dirección de la Onda")]
    public Vector3 waveDirection = Vector3.forward;
    
    [Header("Debug")]
    public bool showDebug = true;
    
    [Header("Onda Personalizada")]
    public GameObject customWaveObject;
    
    private GameObject waveVisualObject;
    private Light waveLight;
    private bool isWaveActive = false;
    
    void Start()
    {
        SetupWaveVisual();
        
        if (showDebug)
        {
            Debug.Log($"MonsterWaveAttack: Script iniciado en {gameObject.name}. Primera onda en {initialDelay} segundos.");
        }
        
        StartCoroutine(WaveCycle());
    }
    
    Vector3 GetWaveOriginPosition()
    {
        return transform.position + transform.TransformDirection(waveOriginOffset);
    }
    
    Vector3 GetWaveDirection()
    {
        return transform.TransformDirection(waveDirection.normalized);
    }
    
    void SetupWaveVisual()
    {
        if (customWaveObject != null)
        {
            waveVisualObject = customWaveObject;
            waveLight = waveVisualObject.GetComponent<Light>();
            
            if (waveLight == null)
            {
                waveLight = waveVisualObject.AddComponent<Light>();
            }
        }
        else
        {
            CreateDefaultWaveVisual();
        }
        
        if (waveLight != null)
        {
            waveLight.type = LightType.Point;
            waveLight.color = waveColor;
            waveLight.intensity = 0f;
            waveLight.range = initialSphereSize * 4f;
            waveLight.shadows = LightShadows.None;
        }
        
        if (waveVisualObject != null)
        {
            waveVisualObject.SetActive(false);
        }
        
        if (showDebug)
        {
            Debug.Log($"MonsterWaveAttack: Objeto visual creado. Distancia: {waveDistance}, Intervalo: {waveInterval}s");
        }
    }
    
    void CreateDefaultWaveVisual()
    {
        waveVisualObject = new GameObject("MonsterWaveLight");
        waveVisualObject.transform.SetParent(transform);
        
        waveLight = waveVisualObject.AddComponent<Light>();
        waveLight.type = LightType.Point;
        waveLight.color = waveColor;
        waveLight.intensity = 0f;
        waveLight.range = initialSphereSize * 4f;
        waveLight.shadows = LightShadows.None;
        
        GameObject waveSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        waveSphere.name = "WaveSphere";
        waveSphere.transform.SetParent(waveVisualObject.transform);
        waveSphere.transform.localPosition = Vector3.zero;
        waveSphere.transform.localRotation = Quaternion.identity;
        waveSphere.transform.localScale = Vector3.one * (initialSphereSize * 2f);
        
        Shader shader = null;
        string[] shaderNames = {
            "Universal Render Pipeline/Lit",
            "Universal Render Pipeline/Unlit",
            "Standard",
            "Unlit/Color",
            "Sprites/Default"
        };
        
        foreach (string shaderName in shaderNames)
        {
            shader = Shader.Find(shaderName);
            if (shader != null)
            {
                if (showDebug)
                {
                    Debug.Log($"MonsterWaveAttack: Usando shader {shaderName}");
                }
                break;
            }
        }
        
        if (shader == null)
        {
            shader = Shader.Find("Sprites/Default");
            if (shader == null)
            {
                Debug.LogError("MonsterWaveAttack: No se pudo encontrar ningún shader válido!");
                return;
            }
        }
        
        Material sphereMaterial = new Material(shader);
        Color materialColor = new Color(waveColor.r, waveColor.g, waveColor.b, 1f);
        sphereMaterial.color = materialColor;
        
        string shaderNameLower = shader.name.ToLower();
        
        if (shaderNameLower.Contains("lit") || shaderNameLower.Contains("standard"))
        {
            if (sphereMaterial.HasProperty("_BaseColor"))
            {
                sphereMaterial.SetColor("_BaseColor", materialColor);
            }
            if (sphereMaterial.HasProperty("_Color"))
            {
                sphereMaterial.SetColor("_Color", materialColor);
            }
            if (sphereMaterial.HasProperty("_Metallic"))
            {
                sphereMaterial.SetFloat("_Metallic", 0f);
            }
            if (sphereMaterial.HasProperty("_Smoothness"))
            {
                sphereMaterial.SetFloat("_Smoothness", 0f);
            }
            if (sphereMaterial.HasProperty("_Glossiness"))
            {
                sphereMaterial.SetFloat("_Glossiness", 0f);
            }
            
            if (sphereMaterial.HasProperty("_EmissionColor"))
            {
                sphereMaterial.EnableKeyword("_EMISSION");
                sphereMaterial.SetColor("_EmissionColor", materialColor * 2f);
                sphereMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            }
        }
        else if (shaderNameLower.Contains("unlit") || shaderNameLower.Contains("sprite"))
        {
            if (sphereMaterial.HasProperty("_Color"))
            {
                sphereMaterial.SetColor("_Color", materialColor);
            }
            if (sphereMaterial.HasProperty("_TintColor"))
            {
                sphereMaterial.SetColor("_TintColor", materialColor);
            }
        }
        
        waveSphere.GetComponent<Renderer>().material = sphereMaterial;
        Destroy(waveSphere.GetComponent<Collider>());
    }
    
    IEnumerator WaveCycle()
    {
        yield return new WaitForSeconds(initialDelay);
        
        while (true)
        {
            LaunchWave();
            yield return new WaitForSeconds(waveInterval);
        }
    }
    
    void LaunchWave()
    {
        if (isWaveActive)
        {
            if (showDebug)
            {
                Debug.LogWarning("MonsterWaveAttack: Ya hay una onda activa, esperando...");
            }
            return;
        }
        
        if (waveVisualObject == null)
        {
            if (showDebug)
            {
                Debug.LogError("MonsterWaveAttack: No hay objeto visual creado!");
            }
            return;
        }
        
        StartCoroutine(AnimateWave());
    }
    
    IEnumerator AnimateWave()
    {
        isWaveActive = true;
        
        Vector3 originPosition = GetWaveOriginPosition();
        Vector3 direction = GetWaveDirection();
        
        waveVisualObject.transform.position = originPosition;
        waveVisualObject.SetActive(true);
        
        if (showDebug)
        {
            Debug.Log($"MonsterWaveAttack: Onda iniciada desde posición {originPosition} en dirección {direction}");
        }
        
        float launchDuration = waveDuration - chargeDuration;
        if (launchDuration < 0.1f) launchDuration = 0.1f;
        
        float chargeElapsed = 0f;
        float chargeStartSize = 0.1f;
        
        while (chargeElapsed < chargeDuration)
        {
            chargeElapsed += Time.deltaTime;
            float chargeProgress = chargeElapsed / chargeDuration;
            
            float currentSize = Mathf.Lerp(chargeStartSize, initialSphereSize, chargeProgress);
            
            if (waveVisualObject.transform.childCount > 0)
            {
                Transform sphere = waveVisualObject.transform.GetChild(0);
                sphere.localScale = Vector3.one * (currentSize * 2f);
            }
            
            if (waveLight != null)
            {
                waveLight.intensity = maxLightIntensity;
                waveLight.range = currentSize * 4f;
                waveLight.color = waveColor;
            }
            
            if (waveVisualObject.transform.childCount > 0)
            {
                Renderer renderer = waveVisualObject.transform.GetChild(0).GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    Color matColor = new Color(waveColor.r, waveColor.g, waveColor.b, 1f);
                    
                    if (renderer.material.HasProperty("_BaseColor"))
                    {
                        renderer.material.SetColor("_BaseColor", matColor);
                    }
                    if (renderer.material.HasProperty("_Color"))
                    {
                        renderer.material.SetColor("_Color", matColor);
                    }
                    if (renderer.material.HasProperty("_TintColor"))
                    {
                        renderer.material.SetColor("_TintColor", matColor);
                    }
                    
                    if (renderer.material.HasProperty("_EmissionColor"))
                    {
                        renderer.material.SetColor("_EmissionColor", waveColor * 2f);
                    }
                }
            }
            
            yield return null;
        }
        
        if (showDebug)
        {
            Debug.Log("MonsterWaveAttack: Fase de carga completada, iniciando lanzamiento");
        }
        
        float launchElapsed = 0f;
        float travelDistance = 0f;
        
        while (launchElapsed < launchDuration)
        {
            launchElapsed += Time.deltaTime;
            float launchProgress = launchElapsed / launchDuration;
            
            travelDistance = launchProgress * waveDistance;
            waveVisualObject.transform.position = originPosition + (direction * travelDistance);
            
            float currentSize = Mathf.Lerp(initialSphereSize, finalSphereSize, launchProgress);
            
            if (waveVisualObject.transform.childCount > 0)
            {
                Transform sphere = waveVisualObject.transform.GetChild(0);
                sphere.localScale = Vector3.one * (currentSize * 2f);
            }
            
            float sphereAlpha = Mathf.Lerp(1f, 0f, launchProgress);
            float lightIntensityProgress = Mathf.Clamp01((launchProgress - 0.7f) / 0.3f);
            float lightIntensity = Mathf.Lerp(maxLightIntensity, 0f, lightIntensityProgress);
            
            if (waveLight != null)
            {
                waveLight.intensity = lightIntensity;
                waveLight.range = currentSize * 4f;
                waveLight.color = waveColor;
            }
            
            if (waveVisualObject.transform.childCount > 0)
            {
                Renderer renderer = waveVisualObject.transform.GetChild(0).GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    Color matColor = new Color(waveColor.r, waveColor.g, waveColor.b, sphereAlpha);
                    
                    if (renderer.material.HasProperty("_BaseColor"))
                    {
                        renderer.material.SetColor("_BaseColor", matColor);
                    }
                    if (renderer.material.HasProperty("_Color"))
                    {
                        renderer.material.SetColor("_Color", matColor);
                    }
                    if (renderer.material.HasProperty("_TintColor"))
                    {
                        renderer.material.SetColor("_TintColor", matColor);
                    }
                    
                    if (renderer.material.HasProperty("_EmissionColor"))
                    {
                        Color emissionColor = new Color(waveColor.r, waveColor.g, waveColor.b, 1f) * (2f * sphereAlpha);
                        renderer.material.SetColor("_EmissionColor", emissionColor);
                    }
                }
            }
            
            yield return null;
        }
        
        waveVisualObject.SetActive(false);
        isWaveActive = false;
        
        if (showDebug)
        {
            Debug.Log("MonsterWaveAttack: Onda completada");
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Vector3 origin = GetWaveOriginPosition();
        Vector3 direction = GetWaveDirection();
        Vector3 endPoint = origin + (direction * waveDistance);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin, endPoint);
        
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.8f);
        Gizmos.DrawSphere(origin, initialSphereSize);
        
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.3f);
        Gizmos.DrawSphere(endPoint, finalSphereSize);
    }
    
    public void TriggerWave()
    {
        LaunchWave();
    }
    
    void OnValidate()
    {
        if (waveLight != null)
        {
            waveLight.color = waveColor;
        }
        
        if (waveVisualObject != null && waveVisualObject.transform.childCount > 0)
        {
            Renderer renderer = waveVisualObject.transform.GetChild(0).GetComponent<Renderer>();
            if (renderer != null && renderer.material != null)
            {
                Color matColor = new Color(waveColor.r, waveColor.g, waveColor.b, 1f);
                
                if (renderer.material.HasProperty("_BaseColor"))
                {
                    renderer.material.SetColor("_BaseColor", matColor);
                }
                if (renderer.material.HasProperty("_Color"))
                {
                    renderer.material.SetColor("_Color", matColor);
                }
                if (renderer.material.HasProperty("_TintColor"))
                {
                    renderer.material.SetColor("_TintColor", matColor);
                }
                
                if (renderer.material.HasProperty("_EmissionColor"))
                {
                    renderer.material.SetColor("_EmissionColor", waveColor * 2f);
                }
            }
        }
    }
}

