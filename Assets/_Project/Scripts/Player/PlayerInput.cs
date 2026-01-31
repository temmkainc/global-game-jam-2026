using UnityEngine;
using Zenject;

public class PlayerInput : MonoBehaviour
{
    private PlayerStateManager _gameStateManager;

    [Inject]
    public void Construct(PlayerStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleMask();
        }
    }

    private void ToggleMask()
    {
        var nextState = _gameStateManager.CurrentState == PlayerState.InMask
            ? PlayerState.NoMask
            : PlayerState.InMask;

        _gameStateManager.SetState(nextState);
    }
}
