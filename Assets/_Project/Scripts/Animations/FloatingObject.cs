using UnityEngine;
using DG.Tweening;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float floatDistance = 0.5f; 
    [SerializeField] private float floatDuration = 1f;   
    [SerializeField] private bool startRandomPhase = true; 

    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;

        float startOffset = startRandomPhase ? Random.Range(0f, floatDuration) : 0f;

        transform.DOMoveY(_initialPosition.y + floatDistance, floatDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetDelay(startOffset);
    }
}
