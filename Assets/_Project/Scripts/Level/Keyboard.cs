using UnityEngine;

public class Keyboard : MonoBehaviour, IInteractable, IHighlightable
{
    [SerializeField] private TypingMinigame _typingMinigame;

    private Outline _outline;
    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    public bool CanBeHighlighted(PlayerInteraction interaction)
    {
        if (interaction.CurrentGrabbedObject != null)
            return false;

        return true;
    }

    public void OnInteract(Player player)
    {
        _typingMinigame.ShowStartScreen();
    }

    public void SetHighlighted(bool value)
    {
        _outline.enabled = value;
    }
}
