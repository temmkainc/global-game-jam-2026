using UnityEngine;
using TMPro;
using DG.Tweening;

public class ObfuscatedText : MonoBehaviour, IObfuscatedText
{
    [Header("UI")]
    [SerializeField] private CanvasGroup _obfuscatedLabelCanvasGroup;
    [SerializeField] private CanvasGroup _translationLabelCanvasGroup;
    [SerializeField] private TMP_Text _translationTMP;
    [SerializeField] private TMP_Text _obfuscatedTMP;

    [Header("Data")]
    [SerializeField] private string _translation;

    [Header("Animation")]
    [SerializeField] private float _fadeDuration = 0.25f;
    [SerializeField] private Ease _fadeEase = Ease.OutQuad;

    private Tween _obfuscatedTween;
    private Tween _translationTween;

    private void Awake()
    {
        _obfuscatedTMP.text = _translation;
        _translationTMP.text = _translation;
        _obfuscatedLabelCanvasGroup.alpha = 1f;
        _translationLabelCanvasGroup.alpha = 0f;
    }

    public void OnSightEnter()
    {
        _translationTMP.text = _translation;

        KillTweens();

        _obfuscatedTween = _obfuscatedLabelCanvasGroup
            .DOFade(0f, _fadeDuration)
            .SetEase(_fadeEase);

        _translationTween = _translationLabelCanvasGroup
            .DOFade(1f, _fadeDuration)
            .SetEase(_fadeEase);
    }

    public void OnSightExit()
    {
        KillTweens();

        _obfuscatedTween = _obfuscatedLabelCanvasGroup
            .DOFade(1f, _fadeDuration)
            .SetEase(_fadeEase);

        _translationTween = _translationLabelCanvasGroup
            .DOFade(0f, _fadeDuration)
            .SetEase(_fadeEase);
    }

    private void KillTweens()
    {
        _obfuscatedTween?.Kill();
        _translationTween?.Kill();
    }

    private void OnDisable()
    {
        KillTweens();
    }
}
