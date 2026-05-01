using System;
using DG.Tweening;
using Oculus.Interaction;
using UnityEngine;
using Tween = DG.Tweening.Tween;

public class Hat : MonoBehaviour
{
    public static Action<bool, int> OnHide;
    private static Action<int> OnChosen;
    
    private bool _purchased;
    private bool _wearing;
    
    public int price;
    
    [SerializeField] private Transform shopHatSnap;
    [SerializeField] private Transform botHatSnap;
    
    [SerializeField] private HatBuyable hatBuyable;
    
    
    private Grabbable _grabbable;
    private GrabInteractable _grabInteractable;

    private int _ownID;
    
    private Material _material;
    [SerializeField] private Material blackMat;
    
    private Renderer _renderer;
    
    private Tween _floatingTween;
    private Tween _spinTween;
    
    private const float FloatTime = 5f;
    private const float RotationTime = 10f;

    private bool _grabEventListening;
    private PointSystem _pointSystem;

    private void Start()
    {
        _ownID = gameObject.GetInstanceID();
        _grabbable = GetComponent<Grabbable>();
        _grabInteractable = GetComponentInChildren<GrabInteractable>();
        _renderer = GetComponentInChildren<Renderer>();
        _material = _renderer.material;

        if (botHatSnap is null) Debug.LogError("Bot hat location is null");
        
        SnapToShop();
        Blackout();
        StartSpinning();

        if (!_grabEventListening)
        {
            _grabbable.WhenPointerEventRaised += GrabHat;
            _grabEventListening = true;
        }
        
        _pointSystem = PointSystem.Instance;
    }

    private void OnEnable()
    {
        OnHide += ChangeColour;
        OnChosen += ChangeHat;
        
        
        if (_grabbable)
        {
            _grabEventListening = true;
            _grabbable.WhenPointerEventRaised += GrabHat;
        }
    }

    private void OnDisable()
    {
        OnHide -= ChangeColour;
        OnChosen -= ChangeHat;
        
        
        if (_grabbable)
        {
            _grabbable.WhenPointerEventRaised -= GrabHat;
            _grabEventListening = false;
        }
    }
    
    private void GrabHat(PointerEvent evt)
    {
        switch (evt.Type)
        {
            case PointerEventType.Select:
                ForceDeselect();
                if (_purchased)
                {
                    // Change bot's active hat to this one
                    OnChosen?.Invoke(_ownID);
                }
                else
                {
                    BuyHat();
                }
                break;
        }
    }

    [ContextMenu("Test Buy Hat")]
    private void BuyHat()
    {
        if (_pointSystem.EnoughPoints(price))
        {
            _purchased = true;
            _pointSystem.SpendPoints(price);
            
            // Remove black overlay
            DefaultMat();
            
            hatBuyable.PlaySpendVFX();
            
            // Move the hat to bot snap position
            OnChosen?.Invoke(_ownID);
        }
        else
        {
            ForceDeselect();
            hatBuyable.PlayFailVFX();
        }
    }
    
    private void ChangeHat(int id)
    {
        if (_ownID == id)
        {
            StopSpinning();
            SnapToBot();
            _grabbable.enabled = false;
            _wearing = true;
            return;
        }

        if (!_wearing) return;
        SnapToShop();
        StartSpinning();
        _grabbable.enabled = true;
        _wearing = false;
    }

    private void ChangeColour(bool hide, int id)
    {
        if (_ownID != id) return;
        if (hide)
        {
            Blackout();
        }
        else
        {
            DefaultMat();
        }
    }

    private void StopSpinning()
    {
        _floatingTween?.Kill();
        _spinTween?.Kill();
    }

    private void StartSpinning()
    {
        if (!_floatingTween.IsActive())
        {
            _floatingTween = transform.DOLocalMoveY(0.2f, FloatTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        if (!_spinTween.IsActive())
        {
            _spinTween = transform.DOLocalRotate(new Vector3(0, 360, 0), RotationTime, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }
    }

    private void Blackout()
    {
        _renderer.material = blackMat;
        Debug.Log(_material.color);
    }

    private void DefaultMat()
    {
        _renderer.material = _material;
    }
    
    private void ForceDeselect()
    {
        _grabInteractable.enabled = false;
        _grabInteractable.enabled = true;
    }
    
    private void SnapToBot()
    {
        transform.SetPositionAndRotation(botHatSnap.position, botHatSnap.rotation);
        transform.SetParent(botHatSnap);
    }

    private void SnapToShop()
    {
        transform.SetPositionAndRotation(shopHatSnap.position, shopHatSnap.rotation);
        transform.SetParent(shopHatSnap);
    }
    
}
