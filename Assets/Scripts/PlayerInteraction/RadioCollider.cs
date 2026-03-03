using System;
using UnityEngine;

namespace PlayerInteraction
{
    public class RadioCollider : MonoBehaviour
    {
        private bool _inRange;
        private bool _isTalking;

        private const string PlayerRangeTag = "PlayerInteractable";
        public static event Action OnPlayerTalking;
        public static event Action OnPlayerStoppedTalking;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerRangeTag))
            {
                _inRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(PlayerRangeTag))
            {
                _inRange = false;
            }
        }
        
        void Update()
        {
            if (_inRange && !_isTalking && OVRInput.GetDown(OVRInput.Button.One))
            {
                Debug.Log("InRange and Pressing");
                OnPlayerTalking?.Invoke();
                _isTalking = true;
            } else if (_isTalking && (OVRInput.GetUp(OVRInput.Button.One) || !_inRange))
            {
                Debug.Log("OutRange or StoppedPressing");
                OnPlayerStoppedTalking?.Invoke();
                _isTalking = false;
            }
        }
    }
}
