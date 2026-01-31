using System;
using UnityEngine;
using Zenject;

public class OnlyMaskVisible : MonoBehaviour
{
    [Inject] private PlayerStateManager _playerStateManager;
    [SerializeField] private GameObject _environment;

    private void Awake()
    {
        _playerStateManager.StateChanged += On_PlayerStateChanged;
        _environment.SetActive(false);
    }

    private void On_PlayerStateChanged(PlayerState state)
    {
        if(state == PlayerState.InMask)
        {
            _environment.SetActive(true);
        }
        else
        {
            _environment.SetActive(false);
        }
    }
}
