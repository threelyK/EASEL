
using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{
    private Transform _transform;
    
    private void Start()
    {
        _transform = gameObject.transform;
    }

    public void MoveDoor()
    {
        _transform.DOMove(transform.position + Vector3.down * 3f, 0.5f);
    }
    

}
