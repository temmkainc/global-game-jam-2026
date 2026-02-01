using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using System;
using System.Threading;
using Zenject;

public class NarrationManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _subtitleTMP;
    [SerializeField] private CanvasGroup _subtitleCanvasGroup;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;

    [Header("Timing")]
    [SerializeField] private float _startDelayAfterAnimation = 0.5f;

    [SerializeField] private NarrationSequence _introSequence;

    private CancellationTokenSource _cts;

    [SerializeField] private bool _isSkippingIntro = false;

    [Inject] private Player _player;


    private void Awake()
    {
        _subtitleTMP.text = string.Empty;
    }

    private void Start()
    {
        if (!_isSkippingIntro)
        {
            PlaySequence(_introSequence, 1f, true);
        }
    }

    public void PlaySequence(
        NarrationSequence sequence,
        float animationDuration = 0f,
        bool isIntro = false
    )
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        PlaySequenceAsync(sequence, animationDuration, isIntro, _cts.Token).Forget();
    }

    private async UniTaskVoid PlaySequenceAsync(
        NarrationSequence sequence,
        float animationDuration,
        bool isIntro,
        CancellationToken token
    )
    {
        try
        {
            await UniTask.Delay(
                TimeSpan.FromSeconds(animationDuration),
                cancellationToken: token
            );

            if (isIntro)
            {
                _player.SetInput(false);
                _player.SetEnabledSight(false);
                await UniTask.Delay(
                    TimeSpan.FromSeconds(_startDelayAfterAnimation),
                    cancellationToken: token
                );
            }

            if (sequence.VoiceOver != null)
            {
                _audioSource.clip = sequence.VoiceOver;
                _audioSource.Play();
            }

            foreach (var line in sequence.Lines)
            {
                if (line.DelayBefore > 0f)
                {
                    await UniTask.Delay(
                        TimeSpan.FromSeconds(line.DelayBefore),
                        cancellationToken: token
                    );
                }

                ShowSubtitle(line.Subtitle);

                await UniTask.Delay(
                    TimeSpan.FromSeconds(line.Duration),
                    cancellationToken: token
                );
            }
            if (isIntro)
            {
                _player.SetInput(true);
                _player.SetEnabledSight(true);
            }

            HideSubtitle();
        }
        catch (OperationCanceledException)
        {
            StopAudio();
            HideSubtitle();
        }
    }

    private void ShowSubtitle(string text)
    {
        _subtitleTMP.text = text;
        _subtitleCanvasGroup.alpha = 1f;
    }

    private void HideSubtitle()
    {
        _subtitleCanvasGroup.alpha = 0f;
        _subtitleTMP.text = string.Empty;
    }

    private void StopAudio()
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
