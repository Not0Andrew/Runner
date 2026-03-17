using Cysharp.Threading.Tasks;
using Runner.Config;
using UnityEngine.SceneManagement;

namespace Runner.Flow
{
    public sealed class SceneFlowService : ISceneFlowService
    {
        private readonly SceneFlowSettings _settings;

        public SceneFlowService(SceneFlowSettings settings)
        {
            _settings = settings;
        }

        public UniTask LoadBootstrapAsync()
        {
            return LoadSceneAsync(_settings.BootstrapSceneName);
        }

        public UniTask LoadMenuAsync()
        {
            return LoadSceneAsync(_settings.MenuSceneName);
        }

        public UniTask LoadGameAsync()
        {
            return LoadSceneAsync(_settings.GameSceneName);
        }

        public UniTask ReloadCurrentAsync()
        {
            string current = SceneManager.GetActiveScene().name;
            return LoadSceneAsync(current);
        }

        private static async UniTask LoadSceneAsync(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
                return;

            if (SceneManager.GetActiveScene().name == sceneName)
                return;

            await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
        }
    }
}
