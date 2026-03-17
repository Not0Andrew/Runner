using Runner.Core;
using Runner.Gameplay;
using UnityEngine;
using VContainer;

namespace Runner.Audio
{
    public sealed class RunnerAudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioClip coinClip;
        [SerializeField] private AudioClip gameOverClip;

        private IRunScoreService _runScoreService;
        private IGameStateService _gameStateService;

        [Inject]
        public void Construct(IRunScoreService runScoreService, IGameStateService gameStateService)
        {
            _runScoreService = runScoreService;
            _gameStateService = gameStateService;
        }

        private void OnEnable()
        {
            _runScoreService ??= RunnerRuntimeContext.RunScoreService;
            _gameStateService ??= RunnerRuntimeContext.GameStateService;

            if (_runScoreService != null)
                _runScoreService.CoinCollected += OnCoinCollected;

            if (_gameStateService != null)
                _gameStateService.StateChanged += OnStateChanged;

            if (musicSource != null && musicSource.isPlaying == false)
                musicSource.Play();
        }

        private void OnDisable()
        {
            if (_runScoreService != null)
                _runScoreService.CoinCollected -= OnCoinCollected;

            if (_gameStateService != null)
                _gameStateService.StateChanged -= OnStateChanged;
        }

        private void OnCoinCollected(int _)
        {
            if (sfxSource != null && coinClip != null)
                sfxSource.PlayOneShot(coinClip);
        }

        private void OnStateChanged(GameState state)
        {
            if (state != GameState.GameOver)
                return;

            if (sfxSource != null && gameOverClip != null)
                sfxSource.PlayOneShot(gameOverClip);
        }
    }
}
