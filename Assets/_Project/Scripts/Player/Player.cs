using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSight Sight { get; private set; }
    public PlayerObfuscatedTextChecker ObfuscatedTextChecker { get; private set; }
    public PlayerInteraction Interaction { get; private set; }
    public PlayerMask PlayerMask { get; private set; }
    [field: SerializeField] public Transform HoldPoint { get; private set; }


    private void Awake()
    {
        Sight = GetComponent<PlayerSight>();
        Interaction = GetComponent<PlayerInteraction>();
        PlayerMask = GetComponent<PlayerMask>();
    }
}
