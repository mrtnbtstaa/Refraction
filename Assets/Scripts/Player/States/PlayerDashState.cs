using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float dashTimer;
    private Vector3 dashDirection;
    private PlayerProperties playerProperties;
    private PlayerStamina playerStamina;
    public PlayerDashState(PlayerController playerController, TransitionState changeState) : base(playerController, changeState)
    {
        playerStamina = playerController.playerStamina;
        playerProperties = playerController.playerProperties;
    }

    public override void EnterState()
    {
        playerController.state = ToString();

        dashTimer = playerProperties.dashDuration; 

        if (playerStamina.CanPerformAction(playerProperties.dashStaminaRequired))
        {
            // Consume 20 stamina per dash execution
            playerStamina.Consume(playerProperties.dashStaminaRequired);

            // Apply dash
            ApplyDash(playerController.inputManager.MoveInput);

            // Disable gravity during dash
            playerController.playerMovement.DisableGravity();
        }
        else
        {
            Debug.Log("Not enough stamina cant perform dash");
            changeState.To(playerController.walkingState);
        }

        

    }

    public override void ExitState()
    {
        playerController.playerMovement.EnableGravity(); // Before leaving the dash state enable back the gravity
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

        dashTimer -= Time.deltaTime;

        playerController.playerMovement.Dash(dashDirection, playerController.playerProperties.dashSpeed);

        if (dashTimer <= 0f)
            changeState.To(playerController.idleState);
    }

    private void ApplyDash(Vector2 input)
    {
        dashDirection = input.magnitude > 0
        ? new Vector3(input.x, 0f, input.y)
        : playerController.playerReferences.cameraTarget.forward;

        // Normalize to ensure consistent dash speed in all directions
        dashDirection.Normalize();
    }
}
