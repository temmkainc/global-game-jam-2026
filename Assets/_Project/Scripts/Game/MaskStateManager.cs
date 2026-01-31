using System;
using UnityEngine;
public enum MaskState
{
    Default,
    Rechargable,
}
public class MaskStateManager
{
    public MaskState CurrentState { get; private set; } = MaskState.Default;
    public event Action<MaskState> StateChanged;

    public void SetState(MaskState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        StateChanged?.Invoke(newState);
    }
}
