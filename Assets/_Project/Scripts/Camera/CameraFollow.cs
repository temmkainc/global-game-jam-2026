using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Smoothing")]
    public float positionSmoothTime = 0.1f;
    public float rotationSmoothTime = 0.1f;

    private Vector3 positionVelocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        // Smooth position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            target.position,
            ref positionVelocity,
            positionSmoothTime
        );


    }
}
