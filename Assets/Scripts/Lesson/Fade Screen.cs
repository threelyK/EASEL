using DG.Tweening;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private bool _isFadeIn = true;

    private void Start()
    {
        DoFade();
    }

    [ContextMenu("DoFade")]
    private void DoFade()
    {
        _canvasGroup.alpha = _isFadeIn ? 0 : 1;
        var endValue = _isFadeIn ? 1 : 0;
        
        _canvasGroup.DOFade(endValue, _fadeDuration).SetEase(Ease.Linear);
    }
}