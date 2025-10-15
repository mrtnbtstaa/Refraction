using Unity.Cinemachine;
using UnityEngine;

[System.Serializable]
public class PlayerProperties
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float moveSpeedMultiplier = 0.2f;
    public float jumpForce = 7f;
    public float groundCheckRadius = 0.1f;
    public float dashDuration = 0.3f;
    public float dashSpeed = 15f;
    public float smoothMovement = 0.5f;
    public bool canDoubleJump = false;
    public float rotationSpeed = 0.5f;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    public float gravityMultiplier = 2f;
    public bool isGravityEnabled = true;

    [Header("Stamina Settings")]
    [Range(0, 100)] public float stamina = 100;
    public float staminaDrainRate = 20;
    public float staminaRecoveryRate = 10;
    public float dashStaminaRequired = 20f;

    [Header("References")]
    public Transform groundTransformPosition;
    public LayerMask groundInteractableMask;
    public Transform playerTransform;


    [Header("Interactable Settings")]
    public Transform interactableTransform;
    public float interactRange = 3f;
    public bool isInteractedActive = false;

   
}
