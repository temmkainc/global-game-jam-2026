using UnityEngine;
using Zenject;

public class CameraZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private float _zoomedFOV = 30f;
    [SerializeField] private float _zoomSmoothTime = 0.3f;

    [Header("Hand Animations")]
    [SerializeField] private Animator _handAnimator;
    [Inject] private PlayerStateManager playerStateManager;
    private Camera _camera;
    private float _defaultFOV;
    private float _currentFOV;
    private float _fovVelocity = 0f;
    private bool _isZoomed = false;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _defaultFOV = _camera.fieldOfView;
        _currentFOV = _defaultFOV;
    }

    private void Update()
    {
        bool holdingZoom = Input.GetMouseButton(1);

        if (holdingZoom && !_isZoomed)
        {
            _isZoomed = true;
            if (playerStateManager.CurrentState != PlayerState.InMask)
            {
                _handAnimator.SetTrigger("PointingStart");
            }
            _handAnimator.SetBool("IsPointing", true);
        }

        if (!holdingZoom && _isZoomed)
        {
            _isZoomed = false;
            _handAnimator.SetBool("IsPointing", false);
        }

        float targetFOV = holdingZoom ? _zoomedFOV : _defaultFOV;

        _currentFOV = Mathf.SmoothDamp(
            _currentFOV,
            targetFOV,
            ref _fovVelocity,
            _zoomSmoothTime
        );

        _camera.fieldOfView = _currentFOV;
    }
}