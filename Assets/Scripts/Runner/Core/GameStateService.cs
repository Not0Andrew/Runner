using System;

namespace Runner.Core
{
    public sealed class GameStateService : IGameStateService
    {
        public GameState State { get; private set; } = GameState.Menu;

        public event Action<GameState> StateChanged;

        public void EnterMenu() => SetState(GameState.Menu);
        public void StartGame() => SetState(GameState.Playing);
        public void SetGameOver() => SetState(GameState.GameOver);

        public void SetPaused(bool isPaused) => SetState(isPaused ? GameState.Paused : GameState.Playing);

        private void SetState(GameState state)
        {
            if (State == state)
                return;

            State = state;
            StateChanged?.Invoke(State);
        }
    }
}
