using Cysharp.Threading.Tasks;
using DG.Tweening;
using Oculus.Interaction;
using UnityEngine;

public class ButtonVertical: ButtonController
{
    private protected override void Start()
    {
        base.Start();
        _pressedPos = transform.localPosition - new Vector3(0, 0, -0.1f); // 0.03
    }

    private protected override void HandlePoke(InteractableStateChangeArgs args)
    {
        if (args.NewState == InteractableState.Select && !_isPressed)
        {
            _isPressed = true;
            transform.DOLocalMove(_pressedPos, 0.5f).SetEase(Ease.OutCirc);
            ProcessPress();
        } 
        else if (args.NewState == InteractableState.Hover && args.PreviousState == InteractableState.Select)
        {
            transform.DOLocalMove(_initialPos, 0.5f).SetEase(Ease.OutSine);
            _ = Unpress();
        }
    }
    
    private async UniTaskVoid Unpress()
    {
        await UniTask.Delay(2000);
        _isPressed = false;
    }
}
