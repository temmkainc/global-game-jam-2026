using UnityEngine;

[CreateAssetMenu(menuName = "Narration/Narration Sequence")]
public class NarrationSequence : ScriptableObject
{
    public AudioClip VoiceOver;
    public NarrationLine[] Lines;
}
