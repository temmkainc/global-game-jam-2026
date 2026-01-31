public interface IHighlightable
{
    void SetHighlighted(bool value);
    bool CanBeHighlighted(PlayerInteraction interaction);
}
