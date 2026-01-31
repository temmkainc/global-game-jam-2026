using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour, IGrabbable, IHighlightable
{
    private Rigidbody _rb;
    private Transform _originalParent;
    private bool _isHeld = false;
    private Outline _outline;
    private int _originalLayer;
    [SerializeField] private float _throwForce = 5f;

    private const int IGNORE_RAYCAST_LAYER = 2; 
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _originalParent = transform.parent;
        _originalLayer = gameObject.layer;
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public bool CanBeHighlighted(PlayerInteraction interaction)
    {
        if (interaction.CurrentGrabbedObject != null)
            return false;

        return true;
    }

    public void SetHighlighted(bool value)
    {
        _outline.enabled = value;
    }
    public void OnInteract(Player player)
    {
        if (!_isHeld)
            Grab(player);
        else
            Throw();
    }

    private void Grab(Player player)
    {
        _isHeld = true;
        _rb.isKinematic = true;
        transform.SetParent(player.HoldPoint); 
        transform.localPosition = Vector3.zero;
        _originalLayer = gameObject.layer;
        gameObject.layer = IGNORE_RAYCAST_LAYER;
    }

    public void Throw(Vector3 direction = default)
    {
        _isHeld = false;
        transform.SetParent(_originalParent);
        gameObject.layer = _originalLayer;
        _rb.isKinematic = false;

        if (direction == Vector3.zero && Camera.main != null)
        {
            direction = Camera.main.transform.forward;
        }

        _rb.AddForce(direction * _throwForce, ForceMode.VelocityChange);
    }
}
