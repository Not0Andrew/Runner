using AdvancedInputSystem;
using Runner.Core;
using UnityEngine;
using VContainer;

namespace PlayerCode
{
    [RequireComponent(typeof(PlayerMovement),typeof(PlayerAnimator), typeof(PlayerInputSystem))]
    public class Player : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private PlayerAnimator _playerAnimator;
        private PlayerInputSystem _playerInputSystem;
        private IGameStateService _gameStateService;

        [Inject]
        public void Construct(IGameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }
        
        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            _playerInputSystem = GetComponent<PlayerInputSystem>();
            
            _playerInputSystem.OnJump += Jump;
            _playerInputSystem.OnDown += Down;
            _playerMovement.GroundedChanged += _playerAnimator.OnGroundedStateChanged;
        }

        private void OnDestroy()
        {
            _playerInputSystem.OnJump -= Jump;
            _playerInputSystem.OnDown -= Down;
            _playerMovement.GroundedChanged -= _playerAnimator.OnGroundedStateChanged;
        }

        private void Jump()
        {
            if (_gameStateService != null && _gameStateService.State != GameState.Playing)
                return;

            _playerMovement.Jump();
            _playerAnimator.StartJumpAnimation();
        }

        private void Down()
        {
            if (_gameStateService != null && _gameStateService.State != GameState.Playing)
                return;

            _playerMovement.Down();
            _playerAnimator.StartDownAnimation();
        }
    }
}
