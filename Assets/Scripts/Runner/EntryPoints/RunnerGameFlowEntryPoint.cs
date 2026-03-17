using Runner.Core;
using Runner.Config;
using Runner.Gameplay;
using VContainer.Unity;

namespace Runner.EntryPoints
{
    public sealed class RunnerGameFlowEntryPoint : IStartable
    {
        private readonly IGameStateService _gameStateService;
        private readonly ISpeedService _speedService;
        private readonly IRunScoreService _runScoreService;
        private readonly IPlayerEffectsService _playerEffectsService;
        private readonly RunnerGameplaySettings _settings;

        public RunnerGameFlowEntryPoint(
            IGameStateService gameStateService,
            ISpeedService speedService,
            IRunScoreService runScoreService,
            IPlayerEffectsService playerEffectsService,
            RunnerGameplaySettings settings)
        {
            _gameStateService = gameStateService;
            _speedService = speedService;
            _runScoreService = runScoreService;
            _playerEffectsService = playerEffectsService;
            _settings = settings;
        }

        public void Start()
        {
            RunnerRuntimeContext.SetGameplayServices(
                _gameStateService,
                _speedService,
                _runScoreService,
                _playerEffectsService);
            _speedService.Reset();
            if (_settings.StartInMenu)
                _gameStateService.EnterMenu();
            else
                _gameStateService.StartGame();
        }
    }
}
