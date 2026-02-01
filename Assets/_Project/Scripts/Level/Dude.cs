using UnityEngine;
using Zenject;

public class Dude : MonoBehaviour, IInteractable, IHighlightable
{
    private Outline _outline;
    [SerializeField] private NarrationSequence _sequence;
    [SerializeField] private NarrationManager _narrationManager;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public bool CanBeHighlighted(PlayerInteraction interaction)
    {
        if (interaction.CurrentGrabbedObject != null)
            return false;

        return true;
    }

    public void OnInteract(Player player)
    {
        Debug.Log("Interacted with Dude");
        if(_narrationManager != null && _sequence != null)
            _narrationManager.PlaySequence(_sequence);
    }

    public void SetHighlighted(bool value)
    {
       _outline.enabled = value;
    }
}
