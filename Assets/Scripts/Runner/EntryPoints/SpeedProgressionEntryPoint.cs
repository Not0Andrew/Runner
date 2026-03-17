using System.Threading;
using System;
using Cysharp.Threading.Tasks;
using Runner.Config;
using Runner.Core;
using VContainer.Unity;

namespace Runner.EntryPoints
{
    public sealed class SpeedProgressionEntryPoint : IAsyncStartable
    {
        private readonly IGameStateService _gameStateService;
        private readonly ISpeedService _speedService;
        private readonly RunnerGameplaySettings _settings;

        public SpeedProgressionEntryPoint(
            IGameStateService gameStateService,
            ISpeedService speedService,
            RunnerGameplaySettings settings)
        {
            _gameStateService = gameStateService;
            _speedService = speedService;
            _settings = settings;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            try
            {
                while (cancellation.IsCancellationRequested == false)
                {
                    await UniTask.WaitUntil(
                        () => _gameStateService.State == GameState.Playing,
                        cancellationToken: cancellation);

                    await UniTask.Delay(
                        (int)(_settings.SpeedIncreaseIntervalSeconds * 1000f),
                        cancellationToken: cancellation);

                    if (_gameStateService.State == GameState.Playing)
                        _speedService.IncreaseStep();
                }
            }
            catch (OperationCanceledException)
            {
                // Scene unload/dispose cancellation is expected.
            }
        }
    }
}
