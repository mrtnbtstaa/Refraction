using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputManager : MonoBehaviour
{
    private PlayerInputSystem_Actions inputAction;
    public static PlayerInputManager Instance { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public event Action OnJump;
    public event Action OnDash;
    public event Action OnSprintStart;
    public event Action OnSprintEnd;
    public event Action OnInteractStart;
    public event Action OnInteractEnd;
    public event Action OnLensMode;
    public event Action OnSwitchColor;
    private float sprintPressTime;
    private float ePressTime;
    private bool isSprinting;
    public bool isInteracting = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        inputAction = new PlayerInputSystem_Actions();
    }
    private void Update()
    {
        MoveInput = inputAction.Player.Move.ReadValue<Vector2>();
    }

    #region OnEnable
    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.Player.Enable();
        Subscribe();
    }

    #endregion

    #region OnDisable 
    private void OnDisable()
    {
        Unsubscribe();
        inputAction.Player.Disable();
        inputAction.Disable();
    }

    #endregion
    private void HandleJump(InputAction.CallbackContext context) => OnJump?.Invoke();
    private void HandleLensMode(InputAction.CallbackContext context) => OnLensMode?.Invoke();
    private void HandleSprintStarted(InputAction.CallbackContext context)
    {
        sprintPressTime = Time.time;
        isSprinting = true;
        OnSprintStart?.Invoke();
    }

    private void HandleSprintCanceled(InputAction.CallbackContext context)
    {
        float pressDuration = Time.time - sprintPressTime;

        if (pressDuration <= 0.2f)
            OnDash?.Invoke();
        else
            OnSprintEnd?.Invoke();

        isSprinting = false;
    }
    private void HandleInteractStart(InputAction.CallbackContext context)
    {
        ePressTime = Time.time;
        OnInteractStart?.Invoke();
    }
    private void HandleInteractEnd(InputAction.CallbackContext context)
    {
        float ePressDuration = Time.time - ePressTime;

        if (ePressDuration <= 0.2f && !isInteracting)
            OnSwitchColor?.Invoke();
        else
            OnInteractEnd?.Invoke();

        isInteracting = false;
    }
    public bool IsRunPressed() => isSprinting;
    public bool IsJumpPressed() => inputAction.Player.Jump.triggered;
    private void Subscribe()
    {
        inputAction.Player.Jump.performed += HandleJump;
        inputAction.Player.Sprint.performed += HandleSprintStarted;
        inputAction.Player.Sprint.canceled += HandleSprintCanceled;
        inputAction.Player.Interact.started += HandleInteractStart;
        inputAction.Player.Interact.canceled += HandleInteractEnd;
        inputAction.Player.Aim.performed += HandleLensMode;
    }

    private void Unsubscribe()
    {
        inputAction.Player.Jump.performed -= HandleJump;
        inputAction.Player.Sprint.performed -= HandleSprintStarted;
        inputAction.Player.Sprint.canceled -= HandleSprintCanceled;
        inputAction.Player.Interact.started -= HandleInteractStart;
        inputAction.Player.Interact.canceled -= HandleInteractEnd;
        inputAction.Player.Aim.performed -= HandleLensMode;
    }
}
