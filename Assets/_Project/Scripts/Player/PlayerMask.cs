using System;
using UnityEngine;
using Zenject;

public class PlayerMask : MonoBehaviour
{
    [Inject] private MaskStateManager _maskStateManager;
    [Inject] private PlayerStateManager _playerStateManager;

    [SerializeField] private float _maxRechargeLevel = 100f;
    [SerializeField] private float _rechargeRate = 0.1f;
    [SerializeField] private float _dechargeRate = 0.5f;
    private float _currentRechargeLevel;
   
    private bool _isRechargable = false;
    private bool _isPlayerInMask = false;

    private PlayerVisuals _playerVisuals;

    private void Awake()
    {
        _maskStateManager.StateChanged += On_MaskStateChanged;
        _playerStateManager.StateChanged += On_PlayerStateChanged;
        _currentRechargeLevel = _maxRechargeLevel;
        _playerVisuals = GetComponent<PlayerVisuals>();
    }

    private void On_MaskStateChanged(MaskState state)
    {
        _isRechargable = true;
        Debug.Log("Mask state changed to Rechargable");
    }

    private void On_PlayerStateChanged(PlayerState state)
    {
        _isPlayerInMask = state == PlayerState.InMask;
        Debug.Log("Player state changed: " + state);
    }

    private void FixedUpdate()
    {
        if(!_isRechargable)
            return;

        if (!_isPlayerInMask)
        {
            _currentRechargeLevel += _rechargeRate * Time.fixedDeltaTime;
        }
        else
        {
            _currentRechargeLevel -= _dechargeRate * Time.fixedDeltaTime;
        }

        _currentRechargeLevel = Mathf.Clamp(_currentRechargeLevel, 0f, _maxRechargeLevel);
        _playerVisuals.UpdateMaskRechargeBar(_currentRechargeLevel / _maxRechargeLevel);
    }
}
