
using UnityEngine;

namespace Avatar
{
    public class FollowHead : MonoBehaviour
    {
        [SerializeField] private Transform _followTarget;
        [SerializeField] private Vector3 _worldOffset;
        [SerializeField] private float smoothing = 8f;

        private void LateUpdate()
        {
            var targetPos = _followTarget.position + _followTarget.TransformDirection(_worldOffset);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(transform.position - _followTarget.position);
        }
    }
}
