using System;
using UnityEngine;

namespace PlayerInteraction
{
    public class RadioCollider : MonoBehaviour
    {
        private bool _inHand;
        private bool _isTalking;
        
        public static event Action OnPlayerTalking;
        public static event Action OnPlayerStoppedTalking;
        
        
        void Update()
        {
            if (_inHand && !_isTalking && OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
            {
                // Debug.Log("InRange and Pressing");
                OnPlayerTalking?.Invoke();
                _isTalking = true;
            } else if (_isTalking && (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger) || !_inHand))
            {
                // Debug.Log("OutRange or StoppedPressing");
                OnPlayerStoppedTalking?.Invoke();
                _isTalking = false;
            }
        }
        
        private void OnEnable()
        {
            RadioActions.OnRadioGrabbed += HandleRadioInHand;
        }

        private void OnDisable()
        {
            RadioActions.OnRadioGrabbed -= HandleRadioInHand;
        }

        private void HandleRadioInHand(bool radioInHand)
        {
            _inHand = radioInHand;
        }
    }
}
