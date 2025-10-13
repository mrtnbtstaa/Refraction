using UnityEngine;

public class PlayerTiredState : PlayerBaseState
{
    public PlayerTiredState(PlayerController playerController, TransitionState changeState) : base(playerController, changeState){}

    public override void EnterState()
    {
        playerController.state = this.ToString();
    }

    public override void ExitState()
    {
    }

    public override void HandleInput()
    {
        Vector2 movement = playerController.inputManager.MoveInput;
        playerMovement.Move(movement, 1f);
    }

    public override void PhysicsState()
    {
        
    }

    public override void UpdateState()
    {
        playerMovement.ApplyGravity();
    }
}
