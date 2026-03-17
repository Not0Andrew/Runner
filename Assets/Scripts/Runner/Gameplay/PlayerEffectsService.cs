using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Runner.Gameplay
{
    public sealed class PlayerEffectsService : IPlayerEffectsService
    {
        private readonly IRunScoreService _runScoreService;

        private CancellationTokenSource _multiplierCts;
        private CancellationTokenSource _invulnerabilityCts;

        public bool IsInvulnerable { get; private set; }
        public event Action<bool> InvulnerabilityChanged;

        public PlayerEffectsService(IRunScoreService runScoreService)
        {
            _runScoreService = runScoreService;
        }

        public void Reset()
        {
            _multiplierCts?.Cancel();
            _invulnerabilityCts?.Cancel();
            _multiplierCts?.Dispose();
            _invulnerabilityCts?.Dispose();
            _multiplierCts = null;
            _invulnerabilityCts = null;

            IsInvulnerable = false;
            _runScoreService.SetMultiplier(1f);
            InvulnerabilityChanged?.Invoke(false);
        }

        public async UniTask ActivateCoinMultiplier(float multiplier, int durationMs)
        {
            _multiplierCts?.Cancel();
            _multiplierCts?.Dispose();
            _multiplierCts = new CancellationTokenSource();

            CancellationToken token = _multiplierCts.Token;
            _runScoreService.SetMultiplier(multiplier);

            try
            {
                await UniTask.Delay(durationMs, cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            _runScoreService.SetMultiplier(1f);
        }

        public async UniTask ActivateInvulnerability(int durationMs)
        {
            _invulnerabilityCts?.Cancel();
            _invulnerabilityCts?.Dispose();
            _invulnerabilityCts = new CancellationTokenSource();
            CancellationToken token = _invulnerabilityCts.Token;

            IsInvulnerable = true;
            InvulnerabilityChanged?.Invoke(true);

            try
            {
                await UniTask.Delay(durationMs, cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            IsInvulnerable = false;
            InvulnerabilityChanged?.Invoke(false);
        }
    }
}
