using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{

    private PlayerProperties playerProperties;
    private PlayerStamina playerStamina;
    public PlayerRunningState(PlayerController playerController, TransitionState changeState) : base(playerController, changeState)
    {
        this.playerProperties = playerController.playerProperties;
        this.playerStamina = playerController.playerStamina;
    }

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

        #region # If Player is not moving switch to idle state
        if (movement.magnitude == 0)
        {
            changeState.To(playerController.idleState);
            return;
        }
        #endregion

        #region If Player is moving and not pressing shift key switch to walking state 
        if(movement.magnitude > 0 && !playerController.inputManager.IsRunPressed())
        {
            changeState.To(playerController.walkingState);
            return;
        }
        #endregion

        playerMovement.Move(movement, playerProperties.moveSpeedMultiplier);

    }

    public override void PhysicsState()
    {
        
    }

    public override void UpdateState()
    {
        playerMovement.ApplyGravity();

        playerStamina.Consume(playerProperties.staminaDrainRate * Time.deltaTime); // Decrease the stamina overtime

        if (playerProperties.stamina <= 0f)
        {
            changeState.To(playerController.tiredState);
            return;
        }

    }
}
