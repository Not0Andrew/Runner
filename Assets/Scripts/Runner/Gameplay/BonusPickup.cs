using Cysharp.Threading.Tasks;
using Runner.Core;
using UnityEngine;
using VContainer;

namespace Runner.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public sealed class BonusPickup : MonoBehaviour
    {
        public enum BonusType
        {
            CoinMultiplier = 0,
            Invulnerability = 1
        }

        [SerializeField] private BonusType type;
        [SerializeField] private float multiplier = 2f;
        [SerializeField] private int durationMs = 5000;

        private IPlayerEffectsService _effectsService;
        private IGameStateService _gameStateService;

        [Inject]
        public void Construct(IPlayerEffectsService effectsService, IGameStateService gameStateService)
        {
            _effectsService = effectsService;
            _gameStateService = gameStateService;
        }

        private void OnTriggerEnter(Collider other)
        {
            IPlayerEffectsService effects = _effectsService ?? RunnerRuntimeContext.PlayerEffectsService;
            IGameStateService gameState = _gameStateService ?? RunnerRuntimeContext.GameStateService;

            if (effects == null)
                return;

            if (gameState != null && gameState.State != GameState.Playing)
                return;

            if (other.GetComponent<PlayerCode.Player>() == null)
                return;

            ApplyBonusAsync(effects).Forget();
            gameObject.SetActive(false);
        }

        private UniTask ApplyBonusAsync(IPlayerEffectsService effects)
        {
            if (type == BonusType.CoinMultiplier)
                return effects.ActivateCoinMultiplier(multiplier, durationMs);

            return effects.ActivateInvulnerability(durationMs);
        }
    }
}
