
using Bots.Stations;
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
        private BlackboardVariable<float> _bgSpeed;
        
        private NavMeshAgent _navAgent;
        private Grabbable _grabbable;
        
        private float timer;
        
        private bool _isSlimed;
        private float _slimeDuration = 3f;
        private float _slimeSpeed;

        [SerializeField] private float _speed = 1f;
        private float _animSpeed = 1f;
        

        private void Awake()
        {
            _grabbable = GetComponent<Grabbable>();
            _navAgent = GetComponent<NavMeshAgent>();
            _bgAgent = GetComponent<BehaviorGraphAgent>();
        }

        private void Start()
        {
            _slimeSpeed = _speed * 0.5f;
            _navAgent.speed = _speed;
            
            // Setting up BehaviourGraph vars
            _bgAgent.GetVariable("Speed", out _bgSpeed);
            _bgSpeed.Value = _speed;
            
            _bgAgent.SetVariableValue("SpeedMagnitude", _animSpeed);
            _bgAgent.GetVariable("Grabbed", out _isGrabbed);
            _bgAgent.GetVariable("Grounded", out _isGrounded);
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
                _navAgent.enabled = true;
            }
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (_isGrabbed == null || _isGrounded) return;
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

        public void MoveToSnap(Transform snapTransform)
        {
            _navAgent.Warp(snapTransform.position);
        }

        private void GotSlimed(GameObject bot)
        {
            if (bot != gameObject) return;
            _isSlimed =  true;
            ChangeSpeed(_slimeSpeed);
        }

        private void ChangeSpeed(float newSpeed)
        {
            _speed = newSpeed;
            _navAgent.speed = _speed;
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
