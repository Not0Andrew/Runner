using Runner.Core;
using Runner.Flow;
using Runner.Gameplay;
using Runner.Progression;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Runner.UI
{
    public sealed class GameOverView : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private GameObject gameOverPanel;

        [Header("Result")]
        [SerializeField] private TMP_Text resultCoinsText;
        [SerializeField] private TMP_InputField playerNameInput;
        [SerializeField] private Button saveResultButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button backToMenuButton;
        [SerializeField] private string defaultPlayerName = "Player";

        [Header("Save feedback")]
        [SerializeField] private TMP_Text saveStateText;

        private IGameStateService _gameStateService;
        private IRunScoreService _runScoreService;
        private ILeaderboardService _leaderboardService;
        private ISceneFlowService _sceneFlowService;

        private bool _isSaved;

        private void Awake()
        {
        }

        private void OnEnable()
        {
            _gameStateService ??= RunnerRuntimeContext.GameStateService;
            _runScoreService ??= RunnerRuntimeContext.RunScoreService;
            _leaderboardService ??= RunnerRuntimeContext.LeaderboardService;
            _sceneFlowService ??= RunnerRuntimeContext.SceneFlowService;

            if (restartButton != null)
                restartButton.onClick.AddListener(RestartRun);

            if (saveResultButton != null)
                saveResultButton.onClick.AddListener(SaveResult);

            if (backToMenuButton != null)
                backToMenuButton.onClick.AddListener(BackToMenu);

            if (_gameStateService != null)
                _gameStateService.StateChanged += OnStateChanged;

            ApplyState(_gameStateService?.State ?? GameState.Playing);
        }

        private void OnDisable()
        {
            if (restartButton != null)
                restartButton.onClick.RemoveListener(RestartRun);

            if (saveResultButton != null)
                saveResultButton.onClick.RemoveListener(SaveResult);

            if (backToMenuButton != null)
                backToMenuButton.onClick.RemoveListener(BackToMenu);

            if (_gameStateService != null)
                _gameStateService.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(GameState state)
        {
            ApplyState(state);
        }

        private void ApplyState(GameState state)
        {
            bool isGameOver = state == GameState.GameOver;

            if (gameplayPanel != null)
                gameplayPanel.SetActive(!isGameOver);

            if (gameOverPanel != null)
                gameOverPanel.SetActive(isGameOver);

            if (isGameOver)
            {
                _isSaved = false;
                UpdateResultText();
                ResetSaveUi();
                SetSaveState(string.Empty);
            }
        }

        private void UpdateResultText()
        {
            if (resultCoinsText == null || _runScoreService == null)
                return;

            resultCoinsText.text = $"Coins: {_runScoreService.Coins}";
        }

        private void SaveResult()
        {
            if (_isSaved || _leaderboardService == null || _runScoreService == null)
                return;

            string playerName = playerNameInput != null ? playerNameInput.text : defaultPlayerName;
            _leaderboardService.SaveRecord(playerName, _runScoreService.Coins);
            _isSaved = true;
            if (saveResultButton != null)
                saveResultButton.interactable = false;
            SetSaveState("Saved");
        }

        private void RestartRun()
        {
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.name);
        }

        private async void BackToMenu()
        {
            _sceneFlowService ??= RunnerRuntimeContext.SceneFlowService;
            if (_sceneFlowService != null)
                await _sceneFlowService.LoadMenuAsync();
        }

        private void SetSaveState(string value)
        {
            if (saveStateText != null)
                saveStateText.text = value;
        }

        private void ResetSaveUi()
        {
            if (saveResultButton != null)
                saveResultButton.interactable = true;

            if (playerNameInput != null && string.IsNullOrWhiteSpace(playerNameInput.text))
                playerNameInput.text = defaultPlayerName;
        }
    }
}
