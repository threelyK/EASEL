using DG.Tweening;
using UnityEngine;

public class SpinZ : MonoBehaviour
{
    [SerializeField] private float _interval = 5f;
    [SerializeField] private bool _onZaxis = true;
    [SerializeField] private bool _anticlockwise;
    
    
    private void Start()
    {
        var rot = _onZaxis ? new Vector3(0, 0, 360) : new Vector3(360, 0, 0);

        if (_anticlockwise) rot *= -1;
        
        gameObject.transform.DOLocalRotate(rot, _interval, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);

        foreach (Transform child in gameObject.transform)
        {
            child.DOLocalRotate(rot * -1, _interval, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }
    }
}
