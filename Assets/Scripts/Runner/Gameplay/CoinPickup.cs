using Runner.Core;
using UnityEngine;
using VContainer;

namespace Runner.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public sealed class CoinPickup : MonoBehaviour
    {
        [SerializeField] private int value = 1;

        private IRunScoreService _runScoreService;
        private IGameStateService _gameStateService;

        [Inject]
        public void Construct(IRunScoreService runScoreService, IGameStateService gameStateService)
        {
            _runScoreService = runScoreService;
            _gameStateService = gameStateService;
        }

        private void OnTriggerEnter(Collider other)
        {
            IGameStateService gameState = _gameStateService ?? RunnerRuntimeContext.GameStateService;
            IRunScoreService scoreService = _runScoreService ?? RunnerRuntimeContext.RunScoreService;

            if (gameState != null && gameState.State != GameState.Playing)
                return;

            if (other.GetComponent<PlayerCode.Player>() == null)
                return;

            scoreService?.AddCoin(value);
            gameObject.SetActive(false);
        }
    }
}
