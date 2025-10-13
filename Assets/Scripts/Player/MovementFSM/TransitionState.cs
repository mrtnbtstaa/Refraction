using UnityEngine;

public class TransitionState
{
    public PlayerBaseState CurrentState { get; private set; }

    public void InitialState(PlayerBaseState initialState)
    {
        CurrentState = initialState;
        CurrentState.EnterState();
    }

    public void To(PlayerBaseState newState)
    {
        // Prevent re-entering the same state
        if (CurrentState == newState) return;

        // Debug.Log($"[FSM] Transition: {CurrentState?.GetType().Name} → {newState.GetType().Name}");

        CurrentState?.ExitState();
        CurrentState = newState;
        newState?.EnterState();
    }

    public void UpdateState()
    {
        CurrentState.HandleInput();
        CurrentState.UpdateState();   
    }
    public void PhysicsState() => CurrentState.PhysicsState();
}
