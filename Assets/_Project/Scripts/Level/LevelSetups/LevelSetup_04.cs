using System;
using UnityEngine;
using Zenject;

public class LevelSetup_04 : LevelSetup
{

    [SerializeField] private TypingMinigame _typingMinigame;

    protected override void Awake()
    {
        _door.Subscribe(_typingMinigame);
    }
}
