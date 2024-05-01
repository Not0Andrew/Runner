using AdvancedInputSystem;
using UnityEngine;

namespace PlayerCode
{
    [RequireComponent(typeof(PlayerMovement),typeof(PlayerAnimator), typeof(PlayerInputSystem))]
    public class Player : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private PlayerAnimator _playerAnimator;
        private PlayerInputSystem _playerInputSystem;
        
        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            _playerInputSystem = GetComponent<PlayerInputSystem>();
            
            _playerInputSystem.OnJump += Jump;
        }

        private void Jump()
        {
            _playerMovement.Jump();
            _playerAnimator.StartJumpAnimation();
        }

        private void Down()
        {
            _playerMovement.Down();
            _playerAnimator.StartDownAnimation();
        }
    }
}
