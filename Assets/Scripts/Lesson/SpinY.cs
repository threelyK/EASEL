using UnityEngine;

public class SpinY : MonoBehaviour
{
    [SerializeField] private float _interval = 5f;
    [SerializeField] private bool _anticlockwise;
    
    
    private void Start()
    {
        var rot = _anticlockwise?

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
