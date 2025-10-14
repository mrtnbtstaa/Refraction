using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.Cinemachine;
using UnityEngine;

public class CameraLensMode : MonoBehaviour
{
    public CinemachineCamera vCam;
    private CinemachineRotationComposer rotationComposer;
    public Material scopeLensMaterial;
    public CanvasGroup scopeCanvasGroup;
    public float normalFOV = 60f;
    public float aimFOV = 30f;
    public float transitionSpeed = 10f;
    public float zoomDuration = 0.2f;
    public Vector3 aimOffset = new Vector3(-2f, 0f, 0f);
    private Vector3 currentAimOffset;
    public bool isLensMode = false;

    private void Awake()
    {
        rotationComposer = vCam.gameObject.GetComponent<CinemachineRotationComposer>();
    }
    public void LensModeActivate()
    {
        isLensMode = !isLensMode;
        StopAllCoroutines();
        StartCoroutine(SmoothZoomCamera(isLensMode ? aimFOV : normalFOV));
    }

    private void Update()
    {
        Vector3 targetOffset = isLensMode ? aimOffset : Vector3.zero;
        currentAimOffset = Vector3.Slerp(currentAimOffset, targetOffset, Time.deltaTime * transitionSpeed);
        rotationComposer.TargetOffset = 
            transform.right * currentAimOffset.x
            + transform.up * currentAimOffset.y
            + transform.forward * currentAimOffset.z;
    }

    private IEnumerator SmoothZoomCamera(float targetFOV)
    {
        float startFOV = vCam.Lens.FieldOfView;
        float time = 0f;

        float startAlpha = scopeLensMaterial.GetFloat("_Alpha_Threshold"); // Current alpha threshold 
        float endAlpha = isLensMode ? 1f : 0f;

        while (time < zoomDuration)
        {
            float t = time / zoomDuration;
            // Smooth the Fov Zoom
            vCam.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t);

            // Smooth the Alpha UI(Fade)
            scopeLensMaterial.SetFloat("_Alpha_Threshold", Mathf.Lerp(startAlpha, endAlpha, t));
            scopeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            time += Time.deltaTime;

            yield return null;
        }

        // To ensure exact FOV at the end
        vCam.Lens.FieldOfView = targetFOV; 

        // To ensure exact alpha at the end
        scopeLensMaterial.SetFloat("_Alpha_Threshold", endAlpha);
        scopeCanvasGroup.alpha = endAlpha;
    }
}
