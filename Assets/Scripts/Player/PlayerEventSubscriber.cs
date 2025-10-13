using UnityEngine;

public class PlayerEventSubscriber 
{
    private PlayerInputManager inputManager;
    private PlayerEventHandler eventHandler;

    public PlayerEventSubscriber(PlayerInputManager inputManager, PlayerEventHandler eventHandler)
    {
        this.inputManager = inputManager;
        this.eventHandler = eventHandler;
    }

    public void Subscribe()
    {
        inputManager.OnJump += eventHandler.HandleJump;
        inputManager.OnDash += eventHandler.HandleDash;
        inputManager.OnSprintStart += eventHandler.HandleSprintStart;
        inputManager.OnSprintEnd += eventHandler.HandleSprintEnd;
        inputManager.OnInteractStart += eventHandler.HandleInteractStart;
        // inputManager.OnInteractEnd += eventHandler.HandleInteractEnd;
    }
    public void Unsubscribe()
    {
        inputManager.OnJump -= eventHandler.HandleJump;
        inputManager.OnDash -= eventHandler.HandleDash;
        inputManager.OnSprintStart -= eventHandler.HandleSprintStart;
        inputManager.OnSprintEnd -= eventHandler.HandleSprintEnd;
        inputManager.OnInteractStart -= eventHandler.HandleInteractStart;
        // inputManager.OnInteractEnd += eventHandler.HandleInteractEnd;
    }

}
