using System.Collections;
using UnityEngine;

public class LensController : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color redCrosshair, blueCrosshair, greenCrosshair, violetCrosshair;
    [ColorUsage(true, true)]
    [SerializeField] private Color redEdgeScope, blueEdgeScope, greenEdgeScope, violetEdgeScope;
    [SerializeField] private Material crosshairMat, edgeMat;
    private Coroutine colorChangeCoroutine;
    [SerializeField] private float maxChangedTime = 0.6f;

    private IEnumerator IEUpdateColor(int currentIndex)
    {

        // Current Colors of the crosshair and scope edge Color
        Color currentCrosshairColor = crosshairMat.GetColor("_Base_Color");
        Color currentEdgeColor = edgeMat.GetColor("_Edge_Color");

        Color targetCrosshairColor = GetCrosshairColor(currentIndex);
        Color targetEdgeColor = GetEdgeColor(currentIndex);

        float time = 0;

        while (time < maxChangedTime)
        {
            // Normalize t 0-1 with easing
            float t = Mathf.SmoothStep(0f, 1f, time / maxChangedTime);

            // Interpolation colors
            Color nextCrosshairColor = Color.Lerp(currentCrosshairColor, targetCrosshairColor, t);
            Color nextEdgeColor = Color.Lerp(currentEdgeColor, targetEdgeColor, t);

            // Apply colors
            crosshairMat.SetColor("_Base_Color", nextCrosshairColor);
            edgeMat.SetColor("_Edge_Color", nextEdgeColor);

            // Increase the time overtime
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure final colors are applied at the end
        crosshairMat.SetColor("_Base_Color", targetCrosshairColor);
        edgeMat.SetColor("_Edge_Color", targetEdgeColor);
    }
    
    public void UpdateColor(int index)
    {
        if (colorChangeCoroutine != null)
            StopCoroutine(colorChangeCoroutine);

        colorChangeCoroutine = StartCoroutine(IEUpdateColor(index));
    }

    private Color GetCrosshairColor(int index)
    {
        return index switch
        {
            0 => redCrosshair,
            1 => blueCrosshair,
            2 => greenCrosshair,
            3 => violetCrosshair,
            _ => Color.white
        };
    }
    
    private Color GetEdgeColor(int index)
    {
        return index switch
        {
            0 => redEdgeScope,
            1 => blueEdgeScope,
            2 => greenEdgeScope,
            3 => violetEdgeScope,
            _ => Color.white
        };
    }

}
