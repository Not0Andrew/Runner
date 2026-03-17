using System;
using Cysharp.Threading.Tasks;

namespace Runner.Gameplay
{
    public interface IPlayerEffectsService
    {
        bool IsInvulnerable { get; }
        event Action<bool> InvulnerabilityChanged;

        UniTask ActivateCoinMultiplier(float multiplier, int durationMs);
        UniTask ActivateInvulnerability(int durationMs);
        void Reset();
    }
}
