using UnityEngine;

namespace PlayerCode
{
    public class PlayerAnimator : MonoBehaviour
    {
        private readonly static int Run = Animator.StringToHash("Run");
        private readonly static int Jump = Animator.StringToHash("Idle");
        
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
        }
        
        public void StartDownAnimation()
        {
            
        }
    }
}
