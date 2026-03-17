using System;

namespace Runner.Core
{
    public interface ISpeedService
    {
        float CurrentSpeed { get; }
        event Action<float> SpeedChanged;

        void Reset();
        void IncreaseStep();
    }
}
