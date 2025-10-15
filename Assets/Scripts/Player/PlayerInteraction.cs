using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerController playerController;
    private Interactable currentInteractable;
    [SerializeField] private float heightOffset = 4f;
    [SerializeField] private float playerFollowSpeed = 3f;
    [SerializeField] private LayerMask interactableMask;
    private Ray interactionRay;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        interactionRay = new Ray();
    }
    public void DetectInteractable()
    {
        interactionRay.origin = playerController.transform.position + Vector3.up * 0.5f;
        interactionRay.direction = playerController.transform.forward;

        if (Physics.Raycast(interactionRay, out RaycastHit hit, playerController.playerProperties.interactRange, interactableMask))
        {

            if(hit.collider != null)
            {
                if (!playerController.playerProperties.isInteractedActive)
                {
                    // Get the hit interactable component
                    Interactable interactable = hit.collider.GetComponent<Interactable>();

                    // Set the found interactable to the currentInteractable
                    currentInteractable = interactable;

                    // Get the transform of the currentInteractable
                    playerController.playerProperties.interactableTransform = currentInteractable.transform;

                    // Set the position of the ui 
                    Vector3 topPosition = new Vector3(
                        playerController.playerProperties.interactableTransform.position.x,
                        playerController.playerProperties.interactableTransform.position.y + heightOffset,
                        playerController.playerProperties.interactableTransform.position.z
                    );
                    // Show the ui text with the set position
                    playerController.uiManager.ShowUiText(topPosition);
                }
            }
        }
        else
        {
            // If no interactable found
            playerController.uiManager.HideUiText();
            playerController.playerProperties.isInteractedActive = false;
            currentInteractable = null;
            playerController.playerProperties.interactableTransform = null;
        }
    }
    
    private void Push()
    {
        if (currentInteractable == null || playerController.inputManager.MoveInput == Vector2.zero) return;

        if (playerController.playerProperties.isInteractedActive)
        {
            // Convert input to world-space push direction
            Vector3 pushDir = (playerController.transform.forward * playerController.inputManager.MoveInput.y + playerController.transform.right * playerController.inputManager.MoveInput.x).normalized;

            //Push the object
            currentInteractable.Push(pushDir);
            
            playerController.playerMovement.Move(playerController.inputManager.MoveInput, playerFollowSpeed);
        }

    }

    private void Update()
    {
        DetectInteractable();
    }

    private void FixedUpdate()
    {
        Push();
    }
}