using UnityEngine;
using Zenject;

public class MaskToGrab : MonoBehaviour, IInteractable, IHighlightable
{
    private Outline _outline;
    [Inject] PlayerStateManager _playerStateManager;
    [SerializeField] private GameObject _hint;
    private void Awake()
    {
        _hint.SetActive(false);
        _outline = GetComponent<Outline>();
    }

    public void OnInteract(Player player)
    {
        player.SetEnabledMask(true);
        _playerStateManager.SetState(PlayerState.InMask);
        _hint.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetHighlighted(bool value)
    {
        _outline.OutlineColor = value ? Color.white : Color.black;
    }

    public bool CanBeHighlighted(PlayerInteraction interaction)
    {
        if (interaction.CurrentGrabbedObject != null)
            return false;

        return true;
    }
}
