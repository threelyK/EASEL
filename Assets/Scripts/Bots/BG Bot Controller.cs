
using System;
using Bots.Stations;
using Oculus.Interaction;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Bots
{
    public class BGBotController : MonoBehaviour
    {
        // Too dependent on behaviour graph stuff
        private BehaviorGraphAgent _bgAgent;
        private BlackboardVariable<bool> _isGrabbed;
        private BlackboardVariable<bool> _isGrounded;
        private BlackboardVariable<bool> _isOnWorkbench;
        private BlackboardVariable<float> _bgSpeed;
        private BlackboardVariable<float> _bgSearchRange;
        
        [SerializeField] private float _searchRadius = 5;
        private NavMeshAgent _navAgent;
        private Grabbable _grabbable;
        
        private float timer;
        
        private bool _isSlimed;
        private float _slimeDuration = 3f;
        private float _slimeSpeed;

        [FormerlySerializedAs("_speed")] [SerializeField] private float _startSpeed = 1f;
        private float _animSpeed = 1f;
        

        private void Awake()
        {
            _grabbable = GetComponent<Grabbable>();
            _navAgent = GetComponent<NavMeshAgent>();
            _bgAgent = GetComponent<BehaviorGraphAgent>();
        }

        private void Start()
        {
            _slimeSpeed = _startSpeed * 0.5f;
            
            if (_navAgent) _navAgent.speed = _startSpeed;
            
            // Setting up BehaviourGraph vars
            if (_bgAgent is not null)
            {
                _bgAgent.GetVariable("Speed", out _bgSpeed);
                _bgSpeed.Value = _startSpeed;
            
                _bgAgent.SetVariableValue("SpeedMagnitude", _animSpeed);
                _bgAgent.GetVariable("Grabbed", out _isGrabbed);
                _bgAgent.GetVariable("Grounded", out _isGrounded);
                _bgAgent.GetVariable("OnWorkbench", out _isOnWorkbench);

                if (_bgAgent.GetVariable("SearchRange", out _bgSearchRange))
                {
                    _bgSearchRange.Value = _searchRadius;
                }
                
                return;
            }

            throw new Exception("bgAgent is null");
        }

        private void Update()
        {
            if (_isSlimed)
            {
                timer += Time.deltaTime;
                if (timer < _slimeDuration) return;
                
                _isSlimed = false;
                ChangeSpeed(_slimeSpeed*2);
                timer = 0f;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_isGrabbed == null || _isGrounded) return;
            if (other.gameObject.CompareTag("Ground") && !_isGrabbed)
            {
                _isGrounded.Value = true;
                if (_navAgent) _navAgent.enabled = true;
            }
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (_isGrabbed == null || _isGrounded) return;
            if (other.gameObject.CompareTag("Ground"))
            {
                _isGrounded.Value = false;
                if (_navAgent != null) _navAgent.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (_grabbable != null)
            {
                _grabbable.WhenPointerEventRaised += HandleGrabbed;
            }
            SlimeStation.OnSlimed += GotSlimed;
        }

        private void OnDisable()
        {
            if (_grabbable != null)
            {
                _grabbable.WhenPointerEventRaised -= HandleGrabbed;
            }
            SlimeStation.OnSlimed -= GotSlimed;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _searchRadius);
        }

        public void MoveToSnap(Transform snapTransform)
        {
            if (_navAgent) _navAgent.enabled = false;
            if (_grabbable)
            {
                _grabbable.enabled = false;
                _isGrabbed.Value = false;
            }
            gameObject.transform.position = snapTransform.position;
            gameObject.transform.rotation = snapTransform.rotation;
            if (_grabbable) _grabbable.enabled = true;
        }

        private void GotSlimed(GameObject bot)
        {
            if (bot != gameObject) return;
            _isSlimed =  true;
            ChangeSpeed(_slimeSpeed);
        }

        private void ChangeSpeed(float newSpeed)
        {
            if (_navAgent) _navAgent.speed = newSpeed;
        }

        private void HandleGrabbed(PointerEvent evt)
        {
            switch (evt.Type)
            {
                case PointerEventType.Select:
                    _isGrabbed.Value = true;
                    break;
                case PointerEventType.Unselect:
                    _isGrabbed.Value = false;
                    break;
            }
        }
    }
}
