using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerSight sight;
    [SerializeField] private LayerMask interactableLayer;
    private IInteractable _currentLookTarget;
    private IHighlightable _currentHighlight;
    public IGrabbable CurrentGrabbedObject { get; private set; }

    private Player _player;
    

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        UpdateLookTarget();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void UpdateLookTarget()
    {
        if (sight.TryGetSightHitOnLayerMask(interactableLayer, out RaycastHit hit) &&
            hit.collider.TryGetComponent<IInteractable>(out var interactable))
        {
            if (_currentLookTarget != interactable)
            {
                ClearHighlight();

                _currentLookTarget = interactable;
                Debug.Log($"[Interaction] Looking at: {hit.collider.name}");
                if (hit.collider.TryGetComponent<IHighlightable>(out var highlight))
                {
                    if (highlight.CanBeHighlighted(this))
                    {
                        _currentHighlight = highlight;
                        _currentHighlight.SetHighlighted(true);
                    }
                }
            }

            return;
        }

        ClearCurrentTarget();
    }
    private void ClearHighlight()
    {
        if (_currentHighlight != null)
        {
            _currentHighlight.SetHighlighted(false);
            _currentHighlight = null;
        }
    }
    private void ClearCurrentTarget()
    {
        if (_currentLookTarget == null)
            return;

        Debug.Log("[Interaction] Stopped looking at interactable");

        ClearHighlight();
        _currentLookTarget = null;
    }

    private void TryInteract()
    {
        if (_currentLookTarget is IReceiveGrabbable receiver)
        {
            if (receiver.CanReceive(CurrentGrabbedObject) && CurrentGrabbedObject != null)
            {
                receiver.Receive(CurrentGrabbedObject, _player);
                if (CurrentGrabbedObject is MonoBehaviour mono)
                {
                    mono.gameObject.layer = LayerMask.NameToLayer("Interactable");
                }
                CurrentGrabbedObject = null;
                return;
            } else if (CurrentGrabbedObject == null)
            {
                receiver.Drop();
                return;
            }
        }

        if (CurrentGrabbedObject != null)
        {
            CurrentGrabbedObject.OnInteract(_player);
            if(CurrentGrabbedObject is MonoBehaviour mono)
            {
                mono.gameObject.layer = LayerMask.NameToLayer("Interactable");
            }
            CurrentGrabbedObject = null;
            return;
        }

        if (_currentLookTarget != null)
        {
            _currentLookTarget.OnInteract(_player);

            if (_currentLookTarget is IGrabbable grabbable)
            {
                CurrentGrabbedObject = grabbable;
                if (CurrentGrabbedObject is MonoBehaviour mono)
                {
                    mono.gameObject.layer = LayerMask.NameToLayer("Hold");
                }
            }
        }
    }

}
