using Runner.Core;
using Runner.Gameplay;
using UnityEngine;
using VContainer;

namespace PlayerCode
{
    public class CheckObstacle : MonoBehaviour
    {
        [SerializeField] private LayerMask obstacleLayerMask = ~0;

        private IGameStateService _gameStateService;
        private IPlayerEffectsService _playerEffectsService;

        [Inject]
        public void Construct(IGameStateService gameStateService, IPlayerEffectsService playerEffectsService)
        {
            _gameStateService = gameStateService;
            _playerEffectsService = playerEffectsService;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsObstacleLayer(collision.gameObject.layer))
                _gameStateService?.SetGameOver();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsObstacleLayer(other.gameObject.layer))
                _gameStateService?.SetGameOver();
        }

        private bool IsObstacleLayer(int layer)
        {
            var effectService = _playerEffectsService ?? RunnerRuntimeContext.PlayerEffectsService;
            if (effectService != null && effectService.IsInvulnerable)
                return false;

            return (obstacleLayerMask.value & (1 << layer)) != 0;
        }
    }
}
