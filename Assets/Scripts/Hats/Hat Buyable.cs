using DG.Tweening;
using UnityEngine;

public class HatBuyable : MonoBehaviour
{
    // Hat shop pillar
    [SerializeField] private GameObject _hat;
    private Renderer _renderer;
    
    private Tween _colourTween;
    private Tween _shakeTween;

    private void Start()
    {
        _renderer = _hat.GetComponent<Renderer>();
    }

    public void PlaySpendVFX()
    {
        // Spend VFX
    }

    public void PlayFailVFX()
    {
        if (_colourTween.IsActive() || _shakeTween.IsActive()) return;
        
        var startColour = _renderer.material.color;
        _colourTween = _renderer.material.DOColor(Color.red, 0.75f);
        _shakeTween = _hat.transform.DOShakeRotation(1f, 85f);
        _colourTween.OnComplete(() => _renderer.material.DOColor(startColour, 0.1f));
    }
}
