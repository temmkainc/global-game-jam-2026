using UnityEngine;
using Zenject;

public class MaskRecharge : MonoBehaviour, IInteractable, IHighlightable
{
    private Outline _outline;
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
        player.PlayerMask.Recharge();
    }

    public void SetHighlighted(bool value)
    {
        _outline.enabled = value;
    }
}
