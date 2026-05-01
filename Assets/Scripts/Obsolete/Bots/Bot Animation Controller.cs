
using System;
using Unity.Behavior;
using UnityEngine;

namespace Bots
{
    public class BotAnimationController : MonoBehaviour
    {
        private readonly int IsFalling = Animator.StringToHash("isFalling");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsGrabbed = Animator.StringToHash("isGrabbed");
        private static readonly int IsDeployed = Animator.StringToHash("isDeployed");
        
        private Animator _animator;
        private Rigidbody _rb;

        private BehaviorGraphAgent _bgAgent;
        private BlackboardVariable<bool> _isGrounded;
        private BlackboardVariable<bool> _isGrabbed;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
            
            _bgAgent = GetComponent<BehaviorGraphAgent>();
            _bgAgent.GetVariable("Grounded", out _isGrounded);
            _bgAgent.GetVariable("Grabbed", out _isGrabbed);
        }

        private void Update()
        {
            HandleVelocityAnimations();
        }

        private void OnEnable()
        {
            if (_isGrabbed != null)
            {
                _isGrabbed.OnValueChanged += HandleGrabbed;
            }
        }
        
        private void OnDisable()
        {
            if (_isGrabbed != null)
            {
                _isGrabbed.OnValueChanged -= HandleGrabbed;
            }
        }

        private void HandleVelocityAnimations()
        {
            if (_rb.linearVelocity.magnitude == 0) return;
            if (_isGrabbed.Value) return;
            
            if (!_isGrounded)
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

        private void HandleGrabbed()
        {
            if (!_isGrabbed.Value) return;
            
            _animator.SetBool(IsGrabbed, true);
            _animator.SetBool(IsDeployed, false);
            _animator.SetBool(IsWalking, false);
            _animator.SetBool(IsFalling, false);
        }
        
    }
}
