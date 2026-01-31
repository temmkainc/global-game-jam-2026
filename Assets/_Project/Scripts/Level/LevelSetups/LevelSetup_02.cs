using System;
using UnityEngine;
using Zenject;

public class LevelSetup_02 : LevelSetup
{
    [Inject] private MaskStateManager _maskStateManager;
    [SerializeField] private GameObject _maskRechargeBar;

    protected override void Awake()
    {
        base.Awake();
        _maskRechargeBar.SetActive(false);
        _objective.Completed += On_ObjectiveCompleted;
    }

    private void On_ObjectiveCompleted()
    {
        _maskStateManager.SetState(MaskState.Rechargable);
        _maskRechargeBar.SetActive(true);
    }
}
