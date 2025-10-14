using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Player Current State

    [Header("Player Current State")]
    public String state;
    
    #endregion
    
    #region Player States & Transition States
    private TransitionState changeState;
    public PlayerIdleState idleState;
    public PlayerWalkingState walkingState;
    public PlayerRunningState runningState;
    public PlayerJumpingState jumpingState;
    public PlayerDashState dashState;
    public PlayerTiredState tiredState;

    #endregion

    #region Player Stats & Player References
    public PlayerProperties playerProperties;
    public PlayerReferences playerReferences;
    public CameraLensMode cameraLensMode { get; private set; }
    private Coroutine staminaRecoveryCoroutine = null;
    public PlayerEventHandler playerEventHandler { get; private set; }
    public PlayerEventSubscriber playerEventSubscriber { get; private set; }
    public PlayerInputManager inputManager;
    public UIManager uiManager;
    [field: SerializeField] public PlayerMovement playerMovement { get; private set; }
    public PlayerStamina playerStamina { get; private set; }

    #endregion
    private void OnEnable() => playerEventSubscriber.Subscribe();
    private void OnDisable() => playerEventSubscriber.Unsubscribe();

    private void Awake()
    {
        inputManager = PlayerInputManager.Instance;
        uiManager = UIManager.Instance;

        playerReferences.controller = GetComponent<CharacterController>();
        cameraLensMode = GetComponent<CameraLensMode>();
        playerReferences.cameraTarget = Camera.main.transform;
        playerMovement = new PlayerMovement(playerReferences.controller, playerProperties, playerReferences);
        playerStamina = new PlayerStamina(playerProperties);

        changeState = new TransitionState();
        
        #region States instance creation
        idleState = new PlayerIdleState(this, changeState);
        walkingState = new PlayerWalkingState(this, changeState);
        runningState = new PlayerRunningState(this, changeState);
        jumpingState = new PlayerJumpingState(this, changeState);
        dashState = new PlayerDashState(this, changeState);
        tiredState = new PlayerTiredState(this, changeState);
        #endregion

        playerEventHandler = new PlayerEventHandler(this, changeState);
        playerEventSubscriber = new PlayerEventSubscriber(inputManager, playerEventHandler);

    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        changeState.InitialState(idleState);
    }
    private void Update()
    {
        changeState.UpdateState();

        uiManager.DecreaseStaminaBar(playerProperties.stamina); // Update the UI bar fill

        float recoveryMultiplier = changeState.CurrentState is PlayerTiredState ? playerProperties.staminaRecoveryRate : 7.5f;

        // Start recovering if stamina isn't full and recovery isnt already starting
        if (!playerStamina.IsStaminaFull() && staminaRecoveryCoroutine == null)
        {
            staminaRecoveryCoroutine = StartCoroutine(playerStamina.IncreaseStamina(recoveryMultiplier, (stamina) => uiManager.IncreaseStaminaBar(stamina)));
        }

        // If stamina is full and coroutine still running, stop it
        if(playerStamina.IsStaminaFull() && staminaRecoveryCoroutine != null)
        {
            StopCoroutine(staminaRecoveryCoroutine);
            staminaRecoveryCoroutine = null;
        }
        
    }
    private void FixedUpdate() => changeState.PhysicsState();


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(playerProperties.groundTransformPosition.position, playerProperties.groundCheckRadius);
    }

}
