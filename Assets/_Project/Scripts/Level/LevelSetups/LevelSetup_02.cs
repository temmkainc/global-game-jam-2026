using System;
using UnityEngine;
using Zenject;

public class LevelSetup_02 : LevelSetup
{
    [Inject] private MaskStateManager _maskStateManager;
    protected override void Awake()
    {
        base.Awake();
        _objective.Completed += On_ObjectiveCompleted;
    }

    private void On_ObjectiveCompleted()
    {
        _maskStateManager.SetState(MaskState.Rechargable);
    }
}
