using System;
using UnityEngine;

namespace Runner.Config
{
    [Serializable]
    public sealed class RunnerGameplaySettings
    {
        [Min(0.1f)]
        [SerializeField] private float _initialSpeed = 6f;

        [Min(0.1f)]
        [SerializeField] private float _maxSpeed = 16f;

        [Min(0.1f)]
        [SerializeField] private float _speedIncreaseStep = 0.5f;

        [Min(1f)]
        [SerializeField] private float _speedIncreaseIntervalSeconds = 10f;
        
        [SerializeField] private bool _startInMenu = false;

        public float InitialSpeed => _initialSpeed;
        public float MaxSpeed => _maxSpeed;
        public float SpeedIncreaseStep => _speedIncreaseStep;
        public float SpeedIncreaseIntervalSeconds => _speedIncreaseIntervalSeconds;
        public bool StartInMenu => _startInMenu;
    }
}
