using UnityEngine;
using System; 
public enum PlayerState
{
    NoMask,
    InMask,
}
public class PlayerStateManager
{
    public PlayerState CurrentState { get; private set; } = PlayerState.NoMask;

    public event Action<PlayerState> StateChanged;

    public void SetState(PlayerState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        StateChanged?.Invoke(newState);
    }
}

