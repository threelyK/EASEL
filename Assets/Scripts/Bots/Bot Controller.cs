
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.AI;

namespace Bots
{
    public class BotController : MonoBehaviour
    {
        
        private static readonly int IsFalling = Animator.StringToHash("isFalling");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsGrabbed = Animator.StringToHash("isGrabbed");
        private static readonly int IsDeployed = Animator.StringToHash("isDeployed");

        private bool _isGrounded;
        
        private Animator _animator;
        private Rigidbody _rb;
        [SerializeField] private float _speed = 1f;
        private NavMeshAgent _agent;
        public Vector3 targetPosition;
        private Grabbable _grabbable;
        
        private void HandleVelocityAnimations()
        {
            if (_rb.linearVelocity.magnitude == 0) return;
            
            if (_rb.linearVelocity.y != 0)
            {
                _animator.SetBool(IsFalling, true);
                _isGrounded = true;
            } 
            else
            {
                _animator.SetBool(IsWalking, true);
                _animator.SetBool(IsFalling, false);
            }
        }

        private void UpdateAnimToDeployed()
        {
            _animator.SetBool(IsDeployed, true);
            _animator.SetBool(IsGrabbed, false);
            _animator.SetBool(IsWalking, false);
            _animator.SetBool(IsFalling, false);
        }

        private void UpdateAnimToGrabbed()
        {
            _animator.SetBool(IsGrabbed, true);
            _animator.SetBool(IsDeployed, false);
            _animator.SetBool(IsWalking, false);
            _animator.SetBool(IsFalling, false);
        }

        private void MoveToSnap(Transform snapTransform)
        {
            _agent.Warp(snapTransform.position);
        }

        private void HandleGrabbed(PointerEvent evt)
        {
            switch (evt.Type)
            {
                case PointerEventType.Select:
                    UpdateAnimToGrabbed();
                    break;
                case PointerEventType.Unselect:
                    _isGrounded = false;
                    _agent.enabled = false;
                    break;
            }
        }

        private void GoToDestination(Vector3 targetPos)
        {
            if (_isGrounded && _agent.isActiveAndEnabled) _agent.SetDestination(targetPos);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _isGrounded = true;
                _agent.enabled = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _isGrounded = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Station")) return;
            
            var station = other.gameObject;
            var snapTransform = station.transform;
            
            UpdateAnimToDeployed();
            MoveToSnap(snapTransform);
        }


        private void Awake()
        {
            _grabbable = GetComponent<Grabbable>();
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _agent = GetComponent<NavMeshAgent>();
            _animator =  GetComponent<Animator>();
            
            _agent.speed = _speed;
        }

        private void Update()
        {
            HandleVelocityAnimations();
            GoToDestination(targetPosition);
        }

        private void OnEnable()
        {
            if (_grabbable != null) _grabbable.WhenPointerEventRaised += HandleGrabbed;
        }
        
        private void OnDisable()
        {
            if (_grabbable != null) _grabbable.WhenPointerEventRaised -= HandleGrabbed;
        }
    }
}
