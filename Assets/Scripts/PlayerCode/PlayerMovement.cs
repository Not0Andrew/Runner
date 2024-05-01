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
        
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public void Jump()
        {
            if (CheckJump())
            {
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        public void Down()
        {
            
        }

        private bool CheckJump()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, checkDistance, ~ignoredLayer))
            {
                _rigidbody.velocity = Vector3.zero;
                return true;
            }

            return false;
        }
    }
}
