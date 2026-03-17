using Runner.Gameplay;
using Runner.Flow;
using Runner.Progression;

namespace Runner.Core
{
    public static class RunnerRuntimeContext
    {
        public static IGameStateService GameStateService { get; private set; }
        public static ISpeedService SpeedService { get; private set; }
        public static IRunScoreService RunScoreService { get; private set; }
        public static ILeaderboardService LeaderboardService { get; private set; }
        public static IPlayerEffectsService PlayerEffectsService { get; private set; }
        public static IPersistentData PersistentData { get; private set; }
        public static IDataProvider DataProvider { get; private set; }
        public static Wallet Wallet { get; private set; }
        public static ISceneFlowService SceneFlowService { get; private set; }

        public static void SetGameplayServices(
            IGameStateService gameStateService,
            ISpeedService speedService,
            IRunScoreService runScoreService,
            IPlayerEffectsService playerEffectsService)
        {
            GameStateService = gameStateService;
            SpeedService = speedService;
            RunScoreService = runScoreService;
            PlayerEffectsService = playerEffectsService;
        }

        public static void SetGlobalServices(
            ILeaderboardService leaderboardService,
            IPersistentData persistentData,
            IDataProvider dataProvider,
            Wallet wallet,
            ISceneFlowService sceneFlowService)
        {
            LeaderboardService = leaderboardService;
            PersistentData = persistentData;
            DataProvider = dataProvider;
            Wallet = wallet;
            SceneFlowService = sceneFlowService;
        }
    }
}
