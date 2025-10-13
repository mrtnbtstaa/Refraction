using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private readonly PlayerProperties playerProperties;
    private readonly PlayerStamina playerStamina;
    public PlayerJumpingState(PlayerController playerController, TransitionState changeState) : base(playerController, changeState)
    {
        this.playerProperties = playerController.playerProperties;
        playerStamina = playerController.playerStamina;
    }

    public override void EnterState()
    {
        playerController.state = this.ToString();
        playerMovement.Jump();
        playerProperties.canDoubleJump = true;

        // When entering to the states make sure to subscribe to the events
        playerController.inputManager.OnJump += HandleDoubleJump;
        playerController.inputManager.OnDash += HandleDash;
    }
    public override void ExitState()
    {
        // Before leaving the state make to unsubscribe to the events to prevent memory leaks
        playerController.inputManager.OnJump -= HandleDoubleJump;
        playerController.inputManager.OnDash -= HandleDash;
    }

    public override void HandleInput()
    {
        Vector2 movement = playerController.inputManager.MoveInput;

        if (movement.magnitude > 0 && !playerController.inputManager.IsRunPressed())
            playerMovement.Move(movement, 1f);
        else if (movement.magnitude > 0 && playerController.inputManager.IsRunPressed())
            playerMovement.Move(movement, playerProperties.moveSpeedMultiplier);
    }
    public override void PhysicsState()
    {
       
    }

    public override void UpdateState()
    {
        playerMovement.ApplyGravity();
        /*
            First we are checking if the player is grounded and also if the velocity y <= 0
            then after the player make contact with the ground we will switch state based on the condition
        */
        if (playerController.playerMovement.IsGrounded && playerController.playerMovement.velocity <= 0)
        {
            Vector2 movement = playerController.inputManager.MoveInput;
            if (movement.magnitude > 0 && !playerController.inputManager.IsRunPressed())
                changeState.To(playerController.walkingState);
            else if (movement.magnitude > 0 && playerController.inputManager.IsRunPressed())
                changeState.To(playerController.runningState);
            else
                changeState.To(playerController.idleState);
        }
    }

    private void HandleDoubleJump() => playerMovement.DoubleJump();
    private void HandleDash()
    {
        if (!playerMovement.IsGrounded)
            changeState.To(playerController.dashState);
    }

}
