using DG.Tweening;
using UnityEngine;

public class HatBuyable : MonoBehaviour
{
    // Hat shop pillar
    [SerializeField] private GameObject _hat;
    private Renderer _renderer;

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
        var startColour = _renderer.material.color;
        var redFade = _renderer.material.DOColor(Color.red, 0.75f);
        _hat.transform.DOShakeRotation(1f, 85f);
        redFade.OnComplete(() => _renderer.material.DOColor(startColour, 0.1f));
    }
}
