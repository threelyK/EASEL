using System;
using DG.Tweening;
using Oculus.Interaction;
using UnityEngine;

namespace PlayerInteraction
{
    public class Radio : MonoBehaviour
    {
        private bool _inHand;
        private bool _isTalking;
        // public Transform beltAnchor;
        
        public static event Action OnPlayerTalking;
        public static event Action OnPlayerStoppedTalking;
        
        [SerializeField] private Grabbable _grabbable;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_inHand && !_isTalking && OVRInput.GetDown(OVRInput.Button.Two))
            {
                Debug.Log("In hand and Pressing");
                OnPlayerTalking?.Invoke();
                _isTalking = true;
            } else if (_isTalking && (OVRInput.GetUp(OVRInput.Button.Two) || !_inHand))
            {
                Debug.Log("Not in hand or StoppedPressing");
                OnPlayerStoppedTalking?.Invoke();
                _isTalking = false;
            }
        }

        private void OnEnable()
        {
            _grabbable.WhenPointerEventRaised += HandleGrab;
        }

        private void OnDisable()
        {
            _grabbable.WhenPointerEventRaised -= HandleGrab;
        }
        
        private void HandleGrab(PointerEvent evt)
        {
            switch (evt.Type)
            {
                case PointerEventType.Select:
                    // When grabbed;
                    _inHand = true;
                    
                    break;
                case PointerEventType.Unselect:
                    // When released;
                    _inHand  = false;
                    break;
            }
        }

        private void OnGrab()
        {
            _inHand = true;
            transform.SetParent(null);
            if (_rb) _rb.isKinematic = true;
        }

        /*
        private void OnRelease()
        {
            transform.SetParent(beltAnchor, true);
            if (_rb) _rb.isKinematic = true; // keep physics off while snapping
            Vector3 targetPos = beltAnchor.transform.position;
            transform.position = targetPos;
            transform.SetParent(beltAnchor, false);
            if (_rb) _rb.isKinematic = false;
        }
        */
    }
}
