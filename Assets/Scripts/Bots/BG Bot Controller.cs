
using Oculus.Interaction;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace Bots
{
    public class BGBotController : MonoBehaviour
    {
        private BehaviorGraphAgent _bgAgent;
        private BlackboardVariable<bool> _isGrabbed;
        private BlackboardVariable<bool> _isGrounded;
        
        private NavMeshAgent _navAgent;
        private Grabbable _grabbable;

        [SerializeField] private float _speed = 1f;

        private void Awake()
        {
            _grabbable = GetComponent<Grabbable>();
            _navAgent = GetComponent<NavMeshAgent>();
            _bgAgent = GetComponent<BehaviorGraphAgent>();
            
            // Setting up NavAgent props
            _navAgent.speed = _speed;
        }

        private void Start()
        {
            // Setting up BehaviourGraph vars
            _bgAgent.SetVariableValue("SpeedMagnitude", _speed);
            _bgAgent.GetVariable("Grabbed", out _isGrabbed);
            _bgAgent.GetVariable("Grounded", out _isGrounded);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground") && !_isGrabbed)
            {
                _isGrounded.Value = true;
                _navAgent.enabled = true;
            }
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _isGrounded.Value = false;
                _navAgent.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (_grabbable != null)
            {
                _grabbable.WhenPointerEventRaised += HandleGrabbed;
            }
        }

        private void OnDisable()
        {
            if (_grabbable != null)
            {
                _grabbable.WhenPointerEventRaised -= HandleGrabbed;
            }
        }

        public void MoveToSnap(Transform snapTransform)
        {
            _navAgent.Warp(snapTransform.position);
        }

        private void HandleGrabbed(PointerEvent evt)
        {
            switch (evt.Type)
            {
                case PointerEventType.Select:
                    _isGrabbed.Value = true;
                    break;
                case PointerEventType.Unselect:
                    // Handle released event
                    _isGrabbed.Value = false;
                    break;
            }
        }
        
    }
}
