using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement
{
    private readonly CharacterController controller;
    private readonly PlayerProperties playerProperties;
    private readonly PlayerReferences playerReferences;
    private bool isGrounded;
    private Vector3 currentMoveVelocity = Vector3.zero;
    [HideInInspector] public float velocity;
    private Vector3 smoothMoveVelocity;
   
    public PlayerMovement(CharacterController controller, PlayerProperties playerProperties, PlayerReferences playerReferences)
    {
        this.controller = controller;
        this.playerProperties = playerProperties;
        this.playerReferences = playerReferences;
    }
    public void Jump()
    {
        // If player is grounded
        if (isGrounded)
            velocity = Mathf.Sqrt(playerProperties.jumpForce - 2f * Physics.gravity.y);
        
    }
    
    public void DoubleJump()
    {
        if(!isGrounded && playerProperties.canDoubleJump)
        {
            velocity = Mathf.Sqrt(playerProperties.jumpForce - 2f * Physics.gravity.y);
            playerProperties.canDoubleJump = false;
        }
    }

    public void Move(Vector2 input, float moveSpeedMultiplier)
    {
        Vector3 playerMovementInput = new(input.x, 0f, input.y);

        // Normalize input magnitude to prevent faster diagonal movement
        if (playerMovementInput.magnitude > 1f)
            playerMovementInput.Normalize();

        // Camera forward direction vector
        Vector3 forwardCam = playerReferences.cameraTarget.forward;

        // Project the camera's forward vector onto the horizontal plane (Y=0)
        Vector3 forwardFlat = Vector3.ProjectOnPlane(forwardCam, Vector3.up);

        // Camera right direction vector
        Vector3 rightFlat = playerReferences.cameraTarget.right;
        
        // Relative movement direction
        Vector3 relativeForwardMovement = playerMovementInput.z * forwardFlat;
        Vector3 relativeRightMovement = playerMovementInput.x * rightFlat;

        // Combined movement
        Vector3 targetMovementDirection = relativeForwardMovement + relativeRightMovement;

        // Player rotation
        RotatePlayer(targetMovementDirection);
        
        float targetSpeed = playerMovementInput.magnitude > 0 ? playerProperties.moveSpeed * moveSpeedMultiplier : 0f;

        Vector3 targetVelocity = targetMovementDirection * targetSpeed;

        smoothMoveVelocity = Vector3.SmoothDamp(smoothMoveVelocity, targetVelocity, ref currentMoveVelocity, playerProperties.smoothMovement);

        // Apply movement
        controller.Move(smoothMoveVelocity * Time.deltaTime);
    }

    public void Dash(Vector3 direction, float speed) => Move(new Vector2(direction.x, direction.z), speed);

    public void ApplyGravity()
    {
        if (!playerProperties.isGravityEnabled) return;

        isGrounded = IsGrounded;
        
        if (isGrounded && velocity < 0)
            velocity = -2f;
        else
            velocity += playerProperties.gravity * playerProperties.gravityMultiplier * Time.deltaTime;

        Vector3 moveGravity = new(0f, velocity, 0f);

        controller.Move(moveGravity * Time.deltaTime);
    }
    public void DisableGravity()
    {
        playerProperties.isGravityEnabled = false;
        velocity = 0f;
    }

    public void EnableGravity()
    {
        playerProperties.isGravityEnabled = true;
    }

    public bool IsGrounded => Physics.CheckSphere(playerProperties.groundTransformPosition.position, playerProperties.groundCheckRadius, playerProperties.groundInteractableMask);
    
    public void RotatePlayer(Vector3 movement)
    {
        if (movement == Vector3.zero) return;

        Vector3 direction = new Vector3(movement.x, 0f, movement.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        playerProperties.playerTransform.rotation = Quaternion.Lerp(playerProperties.playerTransform.rotation, targetRotation, playerProperties.rotationSpeed * Time.deltaTime);
    }

}
