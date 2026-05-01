using DG.Tweening;
using TMPro;
using UnityEngine;

public class HatBuyable : MonoBehaviour
{
    // Hat shop pillar

    [SerializeField] private string _hatName = "hat";
    
    [SerializeField] private GameObject _hat;
    [SerializeField] private ParticleSystem _buyVFX;
    
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _ownedText;
    
    
    private Renderer _renderer;
    private Tween _colourTween;
    private Tween _shakeTween;

    private void Start()
    {
        _renderer = _hat.GetComponent<Renderer>();

        var hat = _hat.GetComponent<Hat>();
        
        UpdateNameText(_hatName);
        UpdatePriceText(hat.price);
        UpdateOwnedText(false);
    }

    public void PlaySpendVFX()
    {
        _buyVFX.Play();
        UpdateOwnedText(true);
    }

    public void PlayFailVFX()
    {
        if (_colourTween.IsActive() || _shakeTween.IsActive()) return;
        
        var startColour = _renderer.material.color;
        _colourTween = _renderer.material.DOColor(Color.red, 0.75f);
        _shakeTween = _hat.transform.DOShakeRotation(1f, 85f);
        _colourTween.OnComplete(() => _renderer.material.DOColor(startColour, 0.1f));
    }


    private void UpdateNameText(string hatName)
    {
        _nameText.text = hatName;
    }
    
    private void UpdatePriceText(int price)
    {
        _priceText.text = $"<b>Price</b>: {price} Points";
    }
    
    private void UpdateOwnedText(bool isOwned)
    {
        if (isOwned)
        {
            _ownedText.text = "Owned";
            _ownedText.color = new Color(1f, 0.75f, 0f);
        }
        else
        {
            _ownedText.text = "Not Owned";
            _ownedText.color = Color.white;
        }
    }
}
