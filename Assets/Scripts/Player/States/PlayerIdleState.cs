using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController playerController, TransitionState changeState) : base(playerController, changeState){}

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

        #region # If Player is moving and not pressing shift key switch to walking state
        if (movement.magnitude > 0 && !playerController.inputManager.IsRunPressed())
            changeState.To(playerController.walkingState);
        #endregion

        #region # If Player is moving and pressing shift key switch to running state
        if (movement.magnitude > 0 && playerController.inputManager.IsRunPressed())
            changeState.To(playerController.runningState);
        #endregion

    }

    public override void PhysicsState()
    {
        
    }

    public override void UpdateState()
    {
        playerMovement.ApplyGravity();
    }
}
