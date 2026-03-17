using Cysharp.Threading.Tasks;
using Runner.Core;
using Runner.Flow;
using Runner.Progression;
using UnityEngine;
using VContainer.Unity;

namespace Runner.EntryPoints
{
    public sealed class BootstrapStartupEntryPoint : IStartable
    {
        private readonly ILeaderboardService _leaderboardService;
        private readonly ISceneFlowService _sceneFlowService;
        private readonly IPersistentData _persistentData;
        private readonly IDataProvider _dataProvider;
        private readonly Wallet _wallet;

        public BootstrapStartupEntryPoint(
            ILeaderboardService leaderboardService,
            ISceneFlowService sceneFlowService,
            IPersistentData persistentData,
            IDataProvider dataProvider,
            Wallet wallet)
        {
            _leaderboardService = leaderboardService;
            _sceneFlowService = sceneFlowService;
            _persistentData = persistentData;
            _dataProvider = dataProvider;
            _wallet = wallet;
        }

        public void Start()
        {
            RunnerRuntimeContext.SetGlobalServices(
                _leaderboardService,
                _persistentData,
                _dataProvider,
                _wallet,
                _sceneFlowService);

            StartFlowAsync().Forget();
        }

        private async UniTaskVoid StartFlowAsync()
        {
            if (Application.isPlaying == false)
                return;

            // Delay scene switch until player loop is stable on main thread.
            await UniTask.SwitchToMainThread();
            await UniTask.NextFrame();
            await UniTask.NextFrame();

            await _sceneFlowService.LoadMenuAsync();
        }
    }
}
