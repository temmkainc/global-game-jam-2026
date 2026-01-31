using UnityEngine;

public class PlayerSight : MonoBehaviour
{
    [SerializeField] private float _sightDistance = 10f;
    [SerializeField] private Transform _eyesPoint;
    [SerializeField] private LayerMask _layerMask;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public Ray GetSightRay()
    {
        return new Ray(_eyesPoint.position, _mainCamera.transform.forward);
    }

    public bool TryGetSightHit(out RaycastHit hit)
    {
        return Physics.Raycast(
            GetSightRay(),
            out hit,
            _sightDistance,
            _layerMask
        );
    }

    public bool TryGetSightHitOnLayerMask(LayerMask layerMask, out RaycastHit hit)
    {
        if (!TryGetSightHit(out hit))
            return false;

        return (layerMask.value & (1 << hit.collider.gameObject.layer)) != 0;
    }


#if UNITY_EDITOR
    private void Update()
    {
        DrawDebug();
    }

    private void DrawDebug()
    {
        Ray ray = GetSightRay();

        if (TryGetSightHit(out RaycastHit hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
        }
        else
        {
            Debug.DrawLine(
                ray.origin,
                ray.origin + ray.direction * _sightDistance,
                Color.red
            );
        }
    }
#endif
}
