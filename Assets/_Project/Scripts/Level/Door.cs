using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField] private float openHeight = 3f;
    [SerializeField] private float openDuration = 1f;

    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private bool _isOpen = false;

    private void Awake()
    {
        _closedPosition = transform.position;
        _openPosition = _closedPosition + Vector3.up * openHeight;
    }

    public void Subscribe(ICompletable objective)
    {
        objective.Completed += OpenDoor;
    }

    private void OpenDoor()
    {
        if (_isOpen) return;

        _isOpen = true;
        transform.DOMove(_openPosition, openDuration).SetEase(Ease.OutCubic);
    }
}
