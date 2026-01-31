using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

public class PlayerObfuscatedTextChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _obfuscatedTextLayer;
    [SerializeField] private float _activationDelay = 0.25f;

    private PlayerSight _sight;
    private PlayerStateManager _playerStateManager;
    private IObfuscatedText _current;

    private bool _sightActive = false;
    private CancellationTokenSource _cts;

    [Inject]
    public void Construct(PlayerSight playerSight, PlayerStateManager playerStateManager)
    {
        _sight = playerSight;
        _playerStateManager = playerStateManager;

        _playerStateManager.StateChanged += OnPlayerStateChanged;
    }

    private void Update()
    {
        if (!_sightActive)
            return;

        if (_sight.TryGetSightHitOnLayerMask(_obfuscatedTextLayer, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<IObfuscatedText>(out var text))
            {
                HandleTarget(text);
                return;
            }
        }

        HandleTarget(null);
    }

    private void HandleTarget(IObfuscatedText newTarget)
    {
        if (_current == newTarget)
            return;

        _current?.OnSightExit();
        _current = newTarget;
        _current?.OnSightEnter();
    }

    private void OnPlayerStateChanged(PlayerState newState)
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        if (newState == PlayerState.InMask)
        {
            ActivateSightAfterDelay(_cts.Token).Forget();
        }
        else
        {
            _sightActive = false;
            HandleTarget(null);
        }
    }

    private async UniTaskVoid ActivateSightAfterDelay(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(_activationDelay), cancellationToken: token);
            _sightActive = true;
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void OnDestroy()
    {
        _playerStateManager.StateChanged -= OnPlayerStateChanged;
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
