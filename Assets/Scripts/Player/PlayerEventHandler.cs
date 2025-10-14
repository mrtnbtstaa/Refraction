using UnityEngine;

public class PlayerEventHandler
{
    private TransitionState changeState;
    private PlayerController playerController;
    private PlayerProperties playerProperties;
    public PlayerEventHandler(PlayerController playerController, TransitionState changeState)
    {
        this.changeState = changeState;
        this.playerController = playerController;
        playerProperties = playerController.playerProperties;
    }

    public void HandleJump()
    {
        if (playerController.playerMovement.IsGrounded)
        {
            changeState.To(playerController.jumpingState);
        }
    }

    public void HandleSprintStart()
    {
        if (playerController.playerMovement.IsGrounded)
            changeState.To(playerController.runningState);
    }

    public void HandleSprintEnd()
    {
        if (playerController.playerMovement.IsGrounded && playerController.playerProperties.stamina <= 0)
        {
            changeState.To(playerController.tiredState);
            return;
        }
        if (playerController.playerMovement.IsGrounded && playerController.playerProperties.stamina > 0)
        {
            changeState.To(playerController.walkingState);
            return;
        }
    }

    public void HandleDash()
    {
        if (playerController.playerMovement.IsGrounded && playerController.inputManager.MoveInput != Vector2.zero)
            changeState.To(playerController.dashState);
    }

    public void HandleInteractStart()
    {
        if (playerController.playerProperties.isInteractedActive)
        {
            Debug.Log("Currently interacted");
            return;
        }
        
        Ray ray = new(playerController.transform.position + Vector3.up * 0.5f, playerController.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, playerController.playerProperties.interactRange, LayerMask.GetMask("Interactable")))
        {
            playerController.playerProperties.interactableTransform = hit.transform;
            if (playerController.playerProperties.interactableTransform != null)
            {
                playerController.playerProperties.isInteractedActive = true;
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                    interactable.Interact();
            }
            else return;
        }

    }
    
    public void HandleLensModeToggle()
    {
        Debug.Log("Handle Lens Mode Activated!");
        playerController.cameraLensMode.LensModeActivate();
    }

}
