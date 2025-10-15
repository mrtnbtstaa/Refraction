using System;
using System.Collections;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    [SerializeField] private Material ColorMat;
    
    [ColorUsage(true, true)]
    [SerializeField] private Color fresnelRed, fresnelBlue, fresnelGreen, fresnelViolet;
    [ColorUsage(true, true)]
    [SerializeField] private Color interiorRed, interiorBlue, interiorGreen, interiorViolet;
    [SerializeField] private float maxChangedTime = 2f;
    private Coroutine colorChangeCoroutine;
    
    private IEnumerator IEUpdateColor(int currentIndex)
    {
        float time = 0;

        // Cache the current colors
        Color currentFresnelColor = ColorMat.GetColor("_Fresnel_Color");
        Color currentInteriorColor = ColorMat.GetColor("_Interior_Color");

        // Get the next target color
        Color targetFresnelColor = GetFresnelColorType(currentIndex);
        Color targetInteriorColor = GetInteriorlColorType(currentIndex);

        while (time < maxChangedTime)
        {
            // Normalized time 0-1 with easing
            float t = Mathf.SmoothStep(0f, 1f, time / maxChangedTime);

            // Interpolate colors
            Color nextFresnelColor = Color.Lerp(currentFresnelColor, targetFresnelColor, t);
            Color nextInteriorColor = Color.Lerp(currentInteriorColor, targetInteriorColor, t);

            // Apply colors
            ColorMat.SetColor("_Fresnel_Color", nextFresnelColor);
            ColorMat.SetColor("_Interior_Color", nextInteriorColor);

            time += Time.deltaTime;
            yield return null;
        }
        
        // Ensure final colors are applied at the end
        ColorMat.SetColor("_Fresnel_Color", targetFresnelColor);
        ColorMat.SetColor("_Interior_Color", targetInteriorColor);

    }
    
    public void UpdateColor(int currentIndex)
    {
        // Stop previous transition if still running
        if (colorChangeCoroutine != null)
            StopCoroutine(colorChangeCoroutine);
            
        // Start new transition
        colorChangeCoroutine = StartCoroutine(IEUpdateColor(currentIndex));
    }

    private Color GetFresnelColorType(int index)
    {
        return index switch
        {
            0 => fresnelRed,
            1 => fresnelBlue,
            2 => fresnelGreen,
            3 => fresnelViolet,
            _ => Color.white
        };
    }

    private Color GetInteriorlColorType(int index)
    {
        return index switch
        {
            0 => interiorRed,
            1 => interiorBlue,
            2 => interiorGreen,
            3 => interiorViolet,
            _ => Color.white
        };
    }
    
    public int GetCurrentIndex(int index) => (index + 1) % Enum.GetNames(typeof(ColorType)).Length;

}
