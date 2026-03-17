using System;
using UnityEngine;

namespace Runner.Gameplay
{
    public sealed class RunScoreService : IRunScoreService
    {
        public int Coins { get; private set; }
        public float CoinMultiplier { get; private set; } = 1f;

        public event Action<int> CoinsChanged;
        public event Action<int> CoinCollected;
        public event Action<float> MultiplierChanged;

        public void Reset()
        {
            Coins = 0;
            CoinMultiplier = 1f;
            CoinsChanged?.Invoke(Coins);
            MultiplierChanged?.Invoke(CoinMultiplier);
        }

        public void AddCoin(int baseAmount)
        {
            int sanitized = Mathf.Max(0, baseAmount);
            int gained = Mathf.RoundToInt(sanitized * CoinMultiplier);
            Coins += gained;
            CoinCollected?.Invoke(gained);
            CoinsChanged?.Invoke(Coins);
        }

        public void SetMultiplier(float multiplier)
        {
            float sanitized = Mathf.Max(1f, multiplier);

            if (Mathf.Approximately(CoinMultiplier, sanitized))
                return;

            CoinMultiplier = sanitized;
            MultiplierChanged?.Invoke(CoinMultiplier);
        }
    }
}
