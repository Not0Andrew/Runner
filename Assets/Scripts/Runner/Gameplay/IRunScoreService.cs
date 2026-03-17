using System;

namespace Runner.Gameplay
{
    public interface IRunScoreService
    {
        int Coins { get; }
        float CoinMultiplier { get; }

        event Action<int> CoinsChanged;
        event Action<int> CoinCollected;
        event Action<float> MultiplierChanged;

        void Reset();
        void AddCoin(int baseAmount);
        void SetMultiplier(float multiplier);
    }
}
