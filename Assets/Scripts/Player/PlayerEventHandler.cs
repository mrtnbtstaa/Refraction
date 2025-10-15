using System;
using UnityEngine;

public class PlayerEventHandler
{
    private int index = 0;
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
    

        Ray ray = new(playerController.transform.position + Vector3.up * 0.5f, playerController.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, playerController.playerProperties.interactRange, LayerMask.GetMask("Interactable")))
        {
            if(hit.transform != null)
            {
                playerController.inputManager.isInteracting = true;
                playerController.playerProperties.isInteractedActive = true;
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                    interactable.Interact();
                else
                    return;
            }
        }

    }

    public void HandleInteractEnd()
    {
        Debug.Log("Event Handler Interact End...");
    }

    public void HandleLensModeToggle()
    {
        playerController.cameraLensMode.LensModeActivate();
    }
    public void HandleSwitchColor()
    {
        index = playerController.colorController.GetCurrentIndex(index);
        playerController.colorController.UpdateColor(index);
        playerController.lensController.UpdateColor(index);
    }

}
