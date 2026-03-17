using Runner.Core;
using Runner.Gameplay;
using System;
using VContainer.Unity;

namespace Runner.EntryPoints
{
    public sealed class RunResultCommitEntryPoint : IStartable, IDisposable
    {
        private readonly IGameStateService _gameStateService;
        private readonly IRunScoreService _runScoreService;
        private bool _committed;

        public RunResultCommitEntryPoint(
            IGameStateService gameStateService,
            IRunScoreService runScoreService)
        {
            _gameStateService = gameStateService;
            _runScoreService = runScoreService;
        }

        public void Start()
        {
            _committed = false;
            _gameStateService.StateChanged += OnStateChanged;
        }

        public void Dispose()
        {
            _gameStateService.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(GameState state)
        {
            if (_committed || state != GameState.GameOver)
                return;

            Wallet wallet = RunnerRuntimeContext.Wallet;
            IDataProvider dataProvider = RunnerRuntimeContext.DataProvider;

            if (wallet == null || dataProvider == null)
            {
                _committed = true;
                return;
            }

            if (_runScoreService.Coins > 0)
            {
                wallet.AddCoins(_runScoreService.Coins);
                dataProvider.Save();
            }

            _committed = true;
        }
    }
}
