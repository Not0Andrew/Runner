using Runner.Core;
using UnityEngine;
using VContainer;

namespace Runner.UI
{
    public sealed class GameStateUiRouter : MonoBehaviour
    {
        [SerializeField] private GameObject menuRoot;
        [SerializeField] private GameObject hudRoot;
        [SerializeField] private GameObject gameOverRoot;
        [SerializeField] private UiPanelTransition menuTransition;
        [SerializeField] private UiPanelTransition hudTransition;
        [SerializeField] private UiPanelTransition gameOverTransition;
        [SerializeField] private bool useAnimatedTransitions = true;

        private IGameStateService _gameStateService;
        private bool _isFirstApply = true;

        [Inject]
        public void Construct(IGameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }

        private void OnEnable()
        {
            _gameStateService ??= RunnerRuntimeContext.GameStateService;

            if (_gameStateService != null)
                _gameStateService.StateChanged += Apply;

            Apply(_gameStateService?.State ?? GameState.Playing);
        }

        private void OnDisable()
        {
            if (_gameStateService != null)
                _gameStateService.StateChanged -= Apply;
        }

        private void Apply(GameState state)
        {
            bool instant = _isFirstApply || !useAnimatedTransitions;

            ApplyPanel(menuRoot, menuTransition, state == GameState.Menu, instant);
            ApplyPanel(hudRoot, hudTransition, state == GameState.Playing || state == GameState.Paused, instant);
            ApplyPanel(gameOverRoot, gameOverTransition, state == GameState.GameOver, instant);

            _isFirstApply = false;
        }

        private void ApplyPanel(GameObject target, UiPanelTransition transition, bool active, bool instant)
        {
            if (transition != null)
            {
                transition.SetVisible(active, instant);
                return;
            }

            if (target != null)
                target.SetActive(active);
        }
    }
}
