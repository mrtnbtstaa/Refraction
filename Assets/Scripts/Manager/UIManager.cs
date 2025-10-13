using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Image staminaBar;
    private Camera mainCamera;
    private Coroutine currentCoroutine;
    [SerializeField] private float fadeDuration = 2f;

    [Header("Interaction UI")]
    [SerializeField] public TMP_Text interactText;
    private float maxStamina = 100f;

    private void Start()
    {
        interactText.gameObject.SetActive(true);
        interactText.ForceMeshUpdate();
        interactText.gameObject.SetActive(false);
    }
    private void Awake()
    {
        mainCamera = Camera.main;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        interactText.text = "Press [E] to Interact.";
        interactText.gameObject.SetActive(false);
    }
    public void IncreaseStaminaFill(float stamina) => staminaBar.fillAmount = stamina / maxStamina;
    public void DecreaseStaminaFill(float stamina) => staminaBar.fillAmount = stamina / maxStamina;
    public void ShowUiText(Vector3 worldPosition)
    {
        if (mainCamera == null) return;

        // Show the text if the text is not active
        if (!interactText.gameObject.activeInHierarchy)
        {
            interactText.gameObject.SetActive(true);
            FadeInText();
        }

        // Update position
        interactText.rectTransform.anchoredPosition = worldPosition;


        // interactText.transform.rotation = Quaternion.LookRotation(interactText.transform.position - Camera.main.transform.position);
    }

    public void HideUiText()
    {
        // If the text already active hide it
        if (interactText.gameObject.activeInHierarchy)
        {
            FadeOutText();
            interactText.gameObject.SetActive(false);
        }
    }

    public void UpdateUiTextPosition(Vector3 worldPosition)
    {
        if (mainCamera == null || !interactText.gameObject.activeInHierarchy) return;

        Vector3 screenPoint = mainCamera.WorldToScreenPoint(worldPosition);

        // Check if the point is in front of the camera
        if (screenPoint.z < 0)
        {
            // Hide the text if the object goes behind the camera
            HideUiText();
            return;
        }

        // 3. Set the UI element's position using its RectTransform
        interactText.rectTransform.position = screenPoint;
    }

    public void FadeInText()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(FadeText(0f, 1f));
    }

    public void FadeOutText()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(FadeText(1f, 0f));
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha)
    {
        Color color = interactText.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            interactText.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure final alpha is exact
        interactText.color = new Color(color.r, color.g, color.b, endAlpha);
    }

}
