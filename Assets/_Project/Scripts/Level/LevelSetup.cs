using System;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    [SerializeField] protected Door _door;
    [SerializeField] protected WordCubeReceiver _objective;
    [SerializeField] protected NarrationSequence _sequence;
    [SerializeField] protected NarrationManager _narrationManager;

    protected virtual void Awake()
    {
        _door.Subscribe(_objective);
        _objective.Completed += StartNarrationSequence;
    }

    protected void StartNarrationSequence()
    {
        if (_sequence == null)
            return;

        _narrationManager.PlaySequence(_sequence);
    }
}