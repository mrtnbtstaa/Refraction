using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerController playerController;
    protected TransitionState changeState;
    protected PlayerMovement playerMovement;
    public PlayerBaseState(PlayerController playerController, TransitionState changeState)
    {
        this.playerController = playerController;
        this.changeState = changeState;
        playerMovement = playerController.playerMovement;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void PhysicsState();
    public abstract void ExitState();
    public abstract void HandleInput();
}
