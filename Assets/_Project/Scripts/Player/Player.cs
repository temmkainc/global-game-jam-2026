using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSight Sight { get; private set; }
    public PlayerObfuscatedTextChecker ObfuscatedTextChecker { get; private set; }
    public PlayerInteraction Interaction { get; private set; }
    public PlayerInput Input { get; private set; }
    public PlayerMask PlayerMask { get; private set; }
    [field: SerializeField] public Transform HoldPoint { get; private set; }

    private FirstPersonMovement _firstPersonMovement;
    private Jump _jump;


    private void Awake()
    {
        Sight = GetComponent<PlayerSight>();
        Interaction = GetComponent<PlayerInteraction>();
        PlayerMask = GetComponent<PlayerMask>();
        _jump = GetComponent<Jump>();
        _firstPersonMovement = GetComponent<FirstPersonMovement>();
        Input = GetComponent<PlayerInput>();

        SetEnabledMask(false);
    }

    public void SetInput(bool value)
    {
        _firstPersonMovement.isLocked = !value;
        _jump.enabled = value;
    }

    public void SetEnabledMask(bool value)
    {
        PlayerMask.enabled = value;
        Input.enabled = value;
    }

    public void SetEnabledSight(bool value)
    {
        Sight.enabled = value;
        Interaction.enabled = value;
    }
}
