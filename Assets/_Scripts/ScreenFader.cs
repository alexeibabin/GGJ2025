using _Scripts.Spawner;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private float _fadeTime;
    [SerializeField] private bool _playFadeOutOnAwake;
    
    private void StartFade(EndGameStartedEvent endGameEvent)
    {
        _image.gameObject.SetActive(true);
        _image.DOFade(1, _fadeTime).OnComplete(OnFadeFinished);
    }

    private void FadeOut()
    {
        _image.DOFade(0, _fadeTime).SetEase(Ease.InExpo);
    }

    private void OnFadeFinished()
    {
        Game.EventHub.Notify(new FadeFinishedEvent());
    }

    private void Awake()
    {
        if (_playFadeOutOnAwake)
        {
            FadeOut();
        }
    }

    private void OnEnable()
    {
        Game.EventHub.Subscribe<EndGameStartedEvent>(StartFade);
    }

    private void OnDisable()
    {
        Game.EventHub.Subscribe<EndGameStartedEvent>(StartFade);
    }
}

public struct FadeFinishedEvent : IEvent
{
}
