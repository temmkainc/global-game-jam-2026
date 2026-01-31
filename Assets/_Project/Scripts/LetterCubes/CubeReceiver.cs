using System;
using UnityEngine;

public class CubeReceiver : MonoBehaviour, IReceiveGrabbable, IHighlightable, IInteractable
{
    public event Action<CubeReceiver> StateChanged;

    private Outline _outline;
    private LetterCube _heldCube;
    private char _correctLetter;
    public bool HasCube => _heldCube != null;
    public LetterCube TakeCube()
    {
        if (_heldCube == null)
            return null;

        LetterCube cube = _heldCube;
        _heldCube = null;

        StateChanged?.Invoke(this);

        return cube;
    }
    public bool CanBeHighlighted(PlayerInteraction interaction)
    {
        if (HasCube && interaction.CurrentGrabbedObject != null)
            return false;

        return true;
    }

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void Initialize(int index, char letter)
    {
        _correctLetter = letter;
    }

    public bool IsCorrect()
    {
        if (_heldCube == null)
            return false;

        return _heldCube.Letter == _correctLetter;
    }

    public bool CanReceive(IGrabbable grabbable)
        => _heldCube == null && grabbable is LetterCube;

    public void Receive(IGrabbable grabbable, Player player)
    {
        _heldCube = grabbable as LetterCube;

        _heldCube.transform.SetParent(transform);
        _heldCube.transform.localPosition = Vector3.zero;
        _heldCube.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);

        StateChanged?.Invoke(this);
    }

    public void SetHighlighted(bool value)
    {
        _outline.enabled = value;
    }

    public void OnInteract(Player player)
    {
        
    }

    public void Drop()
    {
        if (_heldCube == null)
            return;

        var cube = _heldCube as Grabbable;
        cube.Throw(-transform.forward);
        _heldCube = null;

        StateChanged?.Invoke(this);
    }
}
