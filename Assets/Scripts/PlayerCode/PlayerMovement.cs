using System;
using UnityEngine;

namespace PlayerCode
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Jump Settings")]
        
        [SerializeField] private float jumpForce;
        [SerializeField] private float checkDistance;
        [SerializeField] private LayerMask ignoredLayer;
        [SerializeField] private Vector3 groundCheckOffset = Vector3.zero;
        
        private Rigidbody _rigidbody;
        private bool _isGrounded;

        public bool IsGrounded => _isGrounded;
        public event Action<bool> GroundedChanged;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            bool wasGrounded = _isGrounded;
            _isGrounded = CheckGrounded();

            if (wasGrounded != _isGrounded)
                GroundedChanged?.Invoke(_isGrounded);
        }
        
        public void Jump()
        {
            if (_isGrounded)
            {
                var velocity = _rigidbody.linearVelocity;
                velocity.y = 0f;
                _rigidbody.linearVelocity = velocity;
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        public void Down()
        {
            
        }

        private bool CheckGrounded()
        {
            return Physics.Raycast(transform.position + groundCheckOffset, Vector3.down, checkDistance, ~ignoredLayer);
        }
    }
}
