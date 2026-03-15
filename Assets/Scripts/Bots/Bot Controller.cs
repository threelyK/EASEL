using System;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
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
        
        private Animator _animator;
        private Rigidbody _rb;
        [SerializeField] private float _speed = 1f;
        private NavMeshAgent _agent;
        public Vector3 targetPosition;
        private HandGrabInteractable _grabbable;
        
        private void HandleVelocityAnimations()
        {
            if (_rb.linearVelocity.magnitude == 0) return;
            
            if (_rb.linearVelocity.y != 0)
            {
                _animator.SetBool(IsFalling, true);
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
            gameObject.transform.position = snapTransform.position;
        }

        private void DetectIfGrabbed(IInteractorView _)
        {
            Debug.Log("Grabbed detected");
            UpdateAnimToGrabbed();
            _agent.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Station")) return;
            
            var station = other.gameObject;
            var snapTransform = station.transform;
            
            UpdateAnimToDeployed();
            MoveToSnap(snapTransform);
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _agent = GetComponent<NavMeshAgent>();
            
            _agent.speed = _speed;
        }

        private void Update()
        {
            HandleVelocityAnimations();
        }
        
        private void LateUpdate()
        {
            _agent.SetDestination(targetPosition);
        }

        private void OnEnable()
        {
            _grabbable.WhenSelectingInteractorViewAdded += DetectIfGrabbed;
        }
    }
}
