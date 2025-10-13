using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public PlayerWalkingState(PlayerController playerController, TransitionState changeState) : base(playerController, changeState){}

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

        #region If Player is not moving switch to idle state
        if (movement.magnitude == 0)
        {
            changeState.To(playerController.idleState);
            return;
        }
        #endregion

        #region If Player is moving and pressing shift key switch to running state 
        if (movement.magnitude > 0 && playerController.inputManager.IsRunPressed())
        {
            changeState.To(playerController.runningState);
            return;
        }
        #endregion

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
