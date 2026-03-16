
using DG.Tweening;
using MCQ_UI;
using Oculus.Interaction;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public string targetID;
    public OptionChoice optionChoice;
    
    private PokeInteractable _pokeInteractable;
    
    private Vector3 _initialPos;
    private Vector3 _pressedPos;
    private bool _isPressed;

    private void HandlePoke(InteractableStateChangeArgs args)
    {
        if (args.NewState == InteractableState.Select && !_isPressed)
        {
            _isPressed = true;
            transform.DOLocalMove(_pressedPos, 0.1f).SetEase(Ease.OutCirc);
            MCQUIManager.OnAnswerSent(optionChoice);
        } 
        else if (args.NewState == InteractableState.Hover && args.PreviousState == InteractableState.Select)
        {
            transform.DOLocalMove(_initialPos, 0.1f).SetEase(Ease.OutSine);
            _isPressed = false;
        }
    }
    
    private void Awake()
    {
        _pokeInteractable = GetComponent<PokeInteractable>();
    }

    private void Start()
    {
        _initialPos = transform.localPosition;
        _pressedPos = transform.localPosition - new Vector3(0, 0.03f, 0);

        _pokeInteractable.WhenStateChanged += HandlePoke; // Put in on enable and disable
    }

    private void OnEnable()
    {
        if (_pokeInteractable != null)
        {
            _pokeInteractable.WhenStateChanged += HandlePoke;
        }
    }
    
    private void OnDisable()
    {
        if (_pokeInteractable != null)
        {
            _pokeInteractable.WhenStateChanged -= HandlePoke;
        }
    }
}
