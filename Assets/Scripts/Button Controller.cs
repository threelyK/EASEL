
using DG.Tweening;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public string targetID;
    
    private PokeInteractable _pokeInteractable;
    
    private protected Vector3 _initialPos;
    private protected Vector3 _pressedPos;
    private protected bool _isPressed;
    [SerializeField] private UnityEvent eventToInvoke;
    
    private protected void HandlePoke(InteractableStateChangeArgs args)
    {
        if (args.NewState == InteractableState.Select && !_isPressed)
        {
            _isPressed = true;
            transform.DOLocalMove(_pressedPos, 0.1f).SetEase(Ease.OutCirc);
            ProcessPress();
        } 
        else if (args.NewState == InteractableState.Hover && args.PreviousState == InteractableState.Select)
        {
            transform.DOLocalMove(_initialPos, 0.1f).SetEase(Ease.OutSine);
            _isPressed = false;
        }
    }

    private protected virtual void ProcessPress()
    {
        eventToInvoke.Invoke();
    }
    
    private protected void Awake()
    {
        _pokeInteractable = GetComponent<PokeInteractable>();
    }

    private protected void Start()
    {
        _initialPos = transform.localPosition;
        _pressedPos = transform.localPosition - new Vector3(0, 0.03f, 0);

        _pokeInteractable.WhenStateChanged += HandlePoke;
    }

    private protected void OnEnable()
    {
        if (_pokeInteractable != null)
        {
            _pokeInteractable.WhenStateChanged += HandlePoke;
        }
    }
    
    private protected void OnDisable()
    {
        if (_pokeInteractable != null)
        {
            _pokeInteractable.WhenStateChanged -= HandlePoke;
        }
    }
}
