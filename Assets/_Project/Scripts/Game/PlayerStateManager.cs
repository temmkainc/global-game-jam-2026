using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public enum PlayerState
{
    NoMask,
    InMask,
}

public class PlayerStateManager
{
    public PlayerState CurrentState { get; private set; } = PlayerState.NoMask;
    public bool CanUseMask { get; private set; } = true;
    public bool CanRechargeMask { get; private set;  } = true;

    public event Action<PlayerState> StateChanged;

    private const float MASK_COOLDOWN_SECONDS = 5f;
    private CancellationTokenSource _cooldownCts;

    public void SetState(PlayerState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        StateChanged?.Invoke(newState);
    }

    public void SetCanUseMask(bool value)
    {
        CanUseMask = value;
        if (CanUseMask)
            return;

        StartCooldown();
    }

    public void SetCanRechargeMask(bool value)
    {
        CanRechargeMask = value;
    }

    private void StartCooldown()
    {
        _cooldownCts?.Cancel();
        _cooldownCts?.Dispose();

        _cooldownCts = new CancellationTokenSource();
        CanRechargeMask = false;

        CooldownAsync(_cooldownCts.Token).Forget();
    }

    private async UniTaskVoid CooldownAsync(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(
                TimeSpan.FromSeconds(MASK_COOLDOWN_SECONDS),
                cancellationToken: token
            );

            CanRechargeMask = true;
        }
        catch (OperationCanceledException)
        {
        }
    }
}


