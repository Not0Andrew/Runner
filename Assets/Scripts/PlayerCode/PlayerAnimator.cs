using UnityEngine;

namespace PlayerCode
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            StartRunAnimation();
        }

        private void StartRunAnimation()
        {
            _animator.SetBool(Run, true);   
        }
        
        public void StartJumpAnimation()
        {
            _animator.SetBool(Run, false);
            _animator.SetTrigger(Jump);
            _animator.SetBool(Grounded, false);
        }
        
        public void StartDownAnimation()
        {
            _animator.SetBool(Run, false);
        }

        public void OnGroundedStateChanged(bool isGrounded)
        {
            _animator.SetBool(Grounded, isGrounded);

            if (isGrounded)
                StartRunAnimation();
        }
    }
}
