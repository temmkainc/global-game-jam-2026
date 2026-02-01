using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

public class OnlyMaskVisible : MonoBehaviour
{
    [Inject] private PlayerStateManager _playerStateManager;
    [SerializeField] private GameObject _environment;
    [SerializeField] private float enableDelay = 0.5f;
    [SerializeField] private float disableDelay = 0.5f;

    private CancellationTokenSource _cts;

    private void Awake()
    {
        _playerStateManager.StateChanged += On_PlayerStateChanged;
        _environment.SetActive(false);
    }

    private void OnDestroy()
    {
        _playerStateManager.StateChanged -= On_PlayerStateChanged;
        _cts?.Cancel();
        _cts?.Dispose();
    }

    private async void On_PlayerStateChanged(PlayerState state)
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        try
        {
            if (state == PlayerState.InMask)
            {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(enableDelay),
                    cancellationToken: _cts.Token
                );

                _environment.SetActive(true);
            }
            else
            {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(disableDelay),
                    cancellationToken: _cts.Token
                );

                _environment.SetActive(false);
            }
        }
        catch (OperationCanceledException)
        {
            // expected when state changes mid-delay
        }
    }
}
