using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    private PlayerController playerController;
    private Rigidbody rb;
    [SerializeField] private float pushForce = 5f;
    private Vector3 currentMoveVelocity = Vector3.zero;
    private Vector3 smoothMovementVelocity;
    [SerializeField] private float smoothTimeMovement = 0.03f;
    private void Awake()
    {
        playerController = GameObject.Find("Player(Liora)").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }
    public void Interact()
    {
        if (playerController.playerProperties.isInteractedActive)
        {
            playerController.uiManager.HideUiText();
        }
    }

    public void Push(Vector3 direction)
    {
        if(rb != null)
        {
            smoothMovementVelocity = Vector3.SmoothDamp(smoothMovementVelocity, direction.normalized * pushForce, ref currentMoveVelocity, smoothTimeMovement);
            rb.linearVelocity = smoothMovementVelocity * pushForce;
        }
    }

}