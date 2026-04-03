using System;
using UnityEngine;

namespace PlayerInteraction
{
    public class Radio : MonoBehaviour
    {
        private bool _isTalking;
        
        public static event Action OnPlayerTalking;
        public static event Action OnPlayerStoppedTalking;
        
        private void Update()
        {
            if (!_isTalking && OVRInput.GetDown(OVRInput.Button.Two))
            {
                Debug.Log("Pressing voice button");
                OnPlayerTalking?.Invoke();
                _isTalking = true;
            } else if (_isTalking && OVRInput.GetUp(OVRInput.Button.Two))
            {
                Debug.Log("Released voice button");
                OnPlayerStoppedTalking?.Invoke();
                _isTalking = false;
            }
        }
    }
}
