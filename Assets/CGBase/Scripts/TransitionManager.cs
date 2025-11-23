using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;
    private Image faderImage;

    [Header("Duration")]
    public float fadeDuration = 1.0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 1. IMPORTANTE: Primero obtenemos el componente antes de usarlo
        faderImage = GetComponent<Image>();

        if (faderImage != null)
        {
            faderImage.color = new Color(0, 0, 0, 0);
            faderImage.raycastTarget = false;
        }
    }

    public void DoTransition(Action taskToDo)
    {
        StartCoroutine(RutinaTransicion(taskToDo));
    }

    // 2. CORRECCIÓN: Sacamos las Corrutinas fuera de "DoTransition"
    IEnumerator RutinaTransicion(Action tarea)
    {
        // 3. CORRECCIÓN: 'raycastTarget' (con T, no con R)
        faderImage.raycastTarget = true;

        // Fade In (A negro)
        yield return StartCoroutine(Fade(0, 1));

        // Ejecutar la tarea (Cambiar escena, cámara, etc.)
        tarea.Invoke();

        // Esperar un frame por seguridad
        yield return null;

        // Fade Out (A transparente)
        yield return StartCoroutine(Fade(1, 0));

        faderImage.raycastTarget = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0;
        while (time < fadeDuration)
        {
            // 4. CORRECCIÓN: += suma el tiempo. Tú tenías = + (asignación)
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);

            // 5. CORRECCIÓN: Usamos la variable 'alpha' calculada, no 'endAlpha'
            faderImage.color = new Color(0, 0, 0, alpha);

            // 6. CORRECCIÓN: Necesario para que el bucle no congele Unity
            yield return null;
        }

        // Aseguramos que termine en el valor exacto al final
        faderImage.color = new Color(0, 0, 0, endAlpha);
    }
}