using System;
using System.Collections.Generic;
using UnityEngine;

public class WordCubeReceiver : MonoBehaviour, ICompletable
{
    public event Action Completed;

    [field: SerializeField] public string Word { get; set; }

    [SerializeField] private CubeReceiver _cubeReceiverPrefab;
    [SerializeField] private float spacing = 1.2f;
    [SerializeField] private int rowCount = -1;
    [SerializeField] private bool rowReverse = false;

    [Header("Optional: Preplaced Receivers")]
    [SerializeField] private List<CubeReceiver> _preplacedReceivers = new();

    private int _wordLength;
    private Transform _receiversContainer;
    private readonly List<CubeReceiver> _receivers = new();

    private bool _isCompleted = false;

    private void Awake()
    {
        _wordLength = Word.Length;

        if (_preplacedReceivers.Count > 0)
        {
            UsePreplacedReceivers();
        }
        else
        {
            _receiversContainer = new GameObject("Receivers").transform;
            _receiversContainer.SetParent(transform);
            _receiversContainer.localPosition = Vector3.zero;

            SpawnCubeReceivers();
        }
    }

    private void UsePreplacedReceivers()
    {
        for (int i = 0; i < _preplacedReceivers.Count; i++)
        {
            CubeReceiver receiver = _preplacedReceivers[i];
            receiver.Initialize(i, Word[i]);
            receiver.StateChanged += OnReceiverStateChanged;
            _receivers.Add(receiver);
        }
    }

    private void SpawnCubeReceivers()
    {
        int effectiveRowCount = rowCount == -1 ? _wordLength : rowCount;

        for (int i = 0; i < _wordLength; i++)
        {
            int row = effectiveRowCount == _wordLength ? 0 : i / effectiveRowCount;
            int col = i % effectiveRowCount;

            float rowOffset = row * -spacing;

            int rowLength = Mathf.Min(effectiveRowCount, _wordLength - row * effectiveRowCount);
            float startOffset = -((rowLength - 1) * spacing) * 0.5f;

            if (rowReverse)
                col = rowLength - 1 - col;

            CubeReceiver receiver = Instantiate(_cubeReceiverPrefab, _receiversContainer);
            receiver.transform.localPosition = new Vector3(startOffset + col * spacing, rowOffset, 0f);
            receiver.Initialize(i, Word[i]);
            receiver.StateChanged += OnReceiverStateChanged;

            _receivers.Add(receiver);
        }
    }

    private bool CheckWordCorrect()
    {
        if (_receivers.Count != _wordLength)
            return false;
        foreach (var receiver in _receivers)
        {
            if (!receiver.IsCorrect())
                return false;
        }
        return true;
    }

    private void OnReceiverStateChanged(CubeReceiver receiver)
    {
        if (_isCompleted)
            return;

        if (!CheckWordCorrect())
            return;

        _isCompleted = true;
        Debug.Log($"[WordPuzzle] Word completed: {Word}");

        Completed?.Invoke();
    }
}
