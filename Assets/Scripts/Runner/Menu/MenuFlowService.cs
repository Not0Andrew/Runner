using Cysharp.Threading.Tasks;
using Runner.Flow;
using UnityEngine;

namespace Runner.Menu
{
    public sealed class MenuFlowService : IMenuFlowService
    {
        private readonly ISceneFlowService _sceneFlowService;

        public MenuFlowService(ISceneFlowService sceneFlowService)
        {
            _sceneFlowService = sceneFlowService;
        }

        public UniTask StartGameAsync()
        {
            return _sceneFlowService.LoadGameAsync();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
