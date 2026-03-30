using UnityEngine;

namespace Avatar
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private Transform targetCamera;
        
        void LateUpdate()
        {
            if (targetCamera == null) return;
            var dir = transform.position - targetCamera.position;
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
