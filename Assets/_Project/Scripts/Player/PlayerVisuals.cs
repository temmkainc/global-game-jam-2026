using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private Image _maskRechargeBarImage;
    [SerializeField] private Animator _playerHandsAnimator;

    private static readonly int EQUIP_MASK_HASH = Animator.StringToHash("EquipMask");
    private static readonly int UNEQUIP_MASK_HASH = Animator.StringToHash("UnequipMask");

    private PlayerStateManager _gameStateManager;

    [Inject]
    public void Construct(PlayerStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
        _gameStateManager.StateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.InMask:
                _playerHandsAnimator.SetTrigger(EQUIP_MASK_HASH);
                break;

            case PlayerState.NoMask:
                _playerHandsAnimator.SetTrigger(UNEQUIP_MASK_HASH);
                break;
        }
    }

    public void UpdateMaskRechargeBar(float value)
    {
        _maskRechargeBarImage.fillAmount = value;
    }

    private void OnDestroy()
    {
        if (_gameStateManager != null)
            _gameStateManager.StateChanged -= OnGameStateChanged;
    }
}
