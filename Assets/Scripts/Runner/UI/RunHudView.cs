using Runner.Gameplay;
using Runner.Core;
using TMPro;
using UnityEngine;
using VContainer;

namespace Runner.UI
{
    public sealed class RunHudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinsValue;
        [SerializeField] private TMP_Text speedValue;
        [SerializeField] private TMP_Text multiplierValue;
        [SerializeField] private TMP_Text invulnerabilityValue;
        [SerializeField] private string invulnerabilityActiveText = "INVULNERABILITY: ON";
        [SerializeField] private string invulnerabilityInactiveText = string.Empty;

        private IRunScoreService _runScoreService;
        private ISpeedService _speedService;
        private IPlayerEffectsService _playerEffectsService;

        [Inject]
        public void Construct(
            IRunScoreService runScoreService,
            ISpeedService speedService,
            IPlayerEffectsService playerEffectsService)
        {
            _runScoreService = runScoreService;
            _speedService = speedService;
            _playerEffectsService = playerEffectsService;
        }

        private void OnEnable()
        {
            _runScoreService ??= RunnerRuntimeContext.RunScoreService;
            _speedService ??= RunnerRuntimeContext.SpeedService;
            _playerEffectsService ??= RunnerRuntimeContext.PlayerEffectsService;

            if (_runScoreService != null)
            {
                _runScoreService.CoinsChanged += OnCoinsChanged;
                _runScoreService.MultiplierChanged += OnMultiplierChanged;
                OnCoinsChanged(_runScoreService.Coins);
                OnMultiplierChanged(_runScoreService.CoinMultiplier);
            }

            if (_speedService != null)
            {
                _speedService.SpeedChanged += OnSpeedChanged;
                OnSpeedChanged(_speedService.CurrentSpeed);
            }

            if (_playerEffectsService != null)
            {
                _playerEffectsService.InvulnerabilityChanged += OnInvulnerabilityChanged;
                OnInvulnerabilityChanged(_playerEffectsService.IsInvulnerable);
            }
        }

        private void OnDisable()
        {
            if (_runScoreService != null)
            {
                _runScoreService.CoinsChanged -= OnCoinsChanged;
                _runScoreService.MultiplierChanged -= OnMultiplierChanged;
            }

            if (_speedService != null)
                _speedService.SpeedChanged -= OnSpeedChanged;

            if (_playerEffectsService != null)
                _playerEffectsService.InvulnerabilityChanged -= OnInvulnerabilityChanged;
        }

        private void OnCoinsChanged(int coins)
        {
            if (coinsValue != null)
                coinsValue.text = coins.ToString();
        }

        private void OnSpeedChanged(float speed)
        {
            if (speedValue != null)
                speedValue.text = $"Speed: {speed:0.0}";
        }

        private void OnMultiplierChanged(float multiplier)
        {
            if (multiplierValue != null)
                multiplierValue.text = multiplier > 1f ? $"x{multiplier:0.0}" : string.Empty;
        }

        private void OnInvulnerabilityChanged(bool isActive)
        {
            if (invulnerabilityValue != null)
                invulnerabilityValue.text = isActive ? invulnerabilityActiveText : invulnerabilityInactiveText;
        }
    }
}
