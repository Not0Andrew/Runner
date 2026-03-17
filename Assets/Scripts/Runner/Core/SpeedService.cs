using System;
using UnityEngine;

namespace Runner.Core
{
    public sealed class SpeedService : ISpeedService
    {
        private readonly float _initialSpeed;
        private readonly float _increaseStep;
        private readonly float _maxSpeed;

        public float CurrentSpeed { get; private set; }
        public event Action<float> SpeedChanged;

        public SpeedService(float initialSpeed, float increaseStep, float maxSpeed)
        {
            _initialSpeed = Mathf.Max(0.1f, initialSpeed);
            _increaseStep = Mathf.Max(0.01f, increaseStep);
            _maxSpeed = Mathf.Max(_initialSpeed, maxSpeed);

            CurrentSpeed = _initialSpeed;
        }

        public void Reset()
        {
            CurrentSpeed = _initialSpeed;
            SpeedChanged?.Invoke(CurrentSpeed);
        }

        public void IncreaseStep()
        {
            float next = Mathf.Min(_maxSpeed, CurrentSpeed + _increaseStep);

            if (Mathf.Approximately(next, CurrentSpeed))
                return;

            CurrentSpeed = next;
            SpeedChanged?.Invoke(CurrentSpeed);
        }
    }
}
