using DG.Tweening;
using UnityEngine;

public class SpinY : MonoBehaviour
{
    [SerializeField] private float _interval = 5f;
    [SerializeField] private bool _anticlockwise;
    
    
    private void Start()
    {
        var rot = _anticlockwise ? new Vector3(0, -360, 0) : new Vector3(0, 360, 0);
        
        gameObject.transform.DOLocalRotate(rot, _interval, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
}
