using UnityEngine;
using Runner.Core;
using Runner.Menu;
using Runner.Customization;
using UnityEngine.UI;

namespace Runner.UI
{
    public sealed class MainMenuView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button leaderboardButton;
        [SerializeField] private Button backFromLeaderboardButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button backFromShopButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button themeAButton;
        [SerializeField] private Button themeBButton;

        [Header("Panels")]
        [SerializeField] private GameObject menuRoot;
        [SerializeField] private GameObject leaderboardRoot;
        [SerializeField] private GameObject shopRoot;

        [Header("Transitions (Optional)")]
        [SerializeField] private UiPanelTransition menuTransition;
        [SerializeField] private UiPanelTransition leaderboardTransition;
        [SerializeField] private UiPanelTransition shopTransition;

        [SerializeField] private LeaderboardView leaderboardView;

        private IMenuFlowService _menuFlowService;
        private bool _isLoading;

        private void OnEnable()
        {
            BindButtons();
            BackToMenu(true);
        }

        private void OnDisable()
        {
            UnbindButtons();
        }

        public async void StartGame()
        {
            if (_isLoading)
                return;

            _isLoading = true;
            SetPanel(menuRoot, menuTransition, false, false);
            SetPanel(leaderboardRoot, leaderboardTransition, false, false);
            SetPanel(shopRoot, shopTransition, false, false);

            _menuFlowService ??= TryBuildFallbackFlowService();
            if (_menuFlowService != null)
                await _menuFlowService.StartGameAsync();

            _isLoading = false;
        }

        public void ShowLeaderboard()
        {
            SetPanel(menuRoot, menuTransition, false, false);
            SetPanel(leaderboardRoot, leaderboardTransition, true, false);
            SetPanel(shopRoot, shopTransition, false, false);

            if (leaderboardView != null)
                leaderboardView.Refresh();
        }

        public void ShowShop()
        {
            SetPanel(menuRoot, menuTransition, false, false);
            SetPanel(leaderboardRoot, leaderboardTransition, false, false);
            SetPanel(shopRoot, shopTransition, true, false);
        }

        public void BackToMenu()
        {
            BackToMenu(false);
        }

        private void BackToMenu(bool instant)
        {
            SetPanel(leaderboardRoot, leaderboardTransition, false, instant);
            SetPanel(shopRoot, shopTransition, false, instant);
            SetPanel(menuRoot, menuTransition, true, instant);
        }

        public void Exit()
        {
            _menuFlowService ??= TryBuildFallbackFlowService();
            _menuFlowService?.ExitGame();
        }

        private static void SetPanel(GameObject root, UiPanelTransition transition, bool active, bool instant)
        {
            if (transition != null)
            {
                transition.SetVisible(active, instant);
                return;
            }

            if (root != null)
                root.SetActive(active);
        }

        private void BindButtons()
        {
            if (startButton != null)
                startButton.onClick.AddListener(StartGame);
            if (leaderboardButton != null)
                leaderboardButton.onClick.AddListener(ShowLeaderboard);
            if (backFromLeaderboardButton != null)
                backFromLeaderboardButton.onClick.AddListener(BackToMenu);
            if (shopButton != null)
                shopButton.onClick.AddListener(ShowShop);
            if (backFromShopButton != null)
                backFromShopButton.onClick.AddListener(BackToMenu);
            if (exitButton != null)
                exitButton.onClick.AddListener(Exit);
            if (themeAButton != null)
                themeAButton.onClick.AddListener(SetThemeA);
            if (themeBButton != null)
                themeBButton.onClick.AddListener(SetThemeB);
        }

        private void UnbindButtons()
        {
            if (startButton != null)
                startButton.onClick.RemoveListener(StartGame);
            if (leaderboardButton != null)
                leaderboardButton.onClick.RemoveListener(ShowLeaderboard);
            if (backFromLeaderboardButton != null)
                backFromLeaderboardButton.onClick.RemoveListener(BackToMenu);
            if (shopButton != null)
                shopButton.onClick.RemoveListener(ShowShop);
            if (backFromShopButton != null)
                backFromShopButton.onClick.RemoveListener(BackToMenu);
            if (exitButton != null)
                exitButton.onClick.RemoveListener(Exit);
            if (themeAButton != null)
                themeAButton.onClick.RemoveListener(SetThemeA);
            if (themeBButton != null)
                themeBButton.onClick.RemoveListener(SetThemeB);
        }

        private static IMenuFlowService TryBuildFallbackFlowService()
        {
            if (RunnerRuntimeContext.SceneFlowService == null)
                return null;

            return new MenuFlowService(RunnerRuntimeContext.SceneFlowService);
        }

        private static void SetThemeA()
        {
            EnvironmentThemeStorage.Set(EnvironmentThemeType.ThemeA);
        }

        private static void SetThemeB()
        {
            EnvironmentThemeStorage.Set(EnvironmentThemeType.ThemeB);
        }
    }
}
