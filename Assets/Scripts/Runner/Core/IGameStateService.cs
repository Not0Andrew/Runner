using System;

namespace Runner.Core
{
    public interface IGameStateService
    {
        GameState State { get; }
        event Action<GameState> StateChanged;

        void EnterMenu();
        void StartGame();
        void SetGameOver();
        void SetPaused(bool isPaused);
    }
}
