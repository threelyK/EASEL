using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerInteraction
{
    public class Radio : MonoBehaviour
    {
        private bool _isTalking;
        private bool _agentReady;
        [SerializeField] private Image micImage;
        [SerializeField] private Sprite micShow;
        [SerializeField] private Sprite micHid;
        
        public static event Action OnPlayerTalking;
        public static event Action OnPlayerStoppedTalking;

        private void Start()
        {
            DimMic();
        }

        private void Update()
        {
            if (!_isTalking && OVRInput.GetDown(OVRInput.Button.Two))
            {
                Debug.Log("Pressing voice button");
                OnPlayerTalking?.Invoke();
                ShowMic();
                _isTalking = true;
            } else if (_isTalking && OVRInput.GetUp(OVRInput.Button.Two))
            {
                Debug.Log("Released voice button");
                OnPlayerStoppedTalking?.Invoke();
                DimMic();
                _isTalking = false;
            }
        }

        private void ShowMic()
        {
            micImage.sprite = micShow;
        }

        private void DimMic()
        {
            micImage.sprite = micHid;
        }
    }
}
