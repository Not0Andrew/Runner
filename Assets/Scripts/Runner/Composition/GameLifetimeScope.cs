using MapCode;
using PlayerCode;
using Runner.Audio;
using Runner.Config;
using Runner.Core;
using Runner.EntryPoints;
using Runner.Gameplay;
using Runner.UI;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [UnityEngine.SerializeField] private RunnerGameplaySettings gameplaySettings = new RunnerGameplaySettings();

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(gameplaySettings);

        builder.Register<IGameStateService, GameStateService>(Lifetime.Singleton);
        builder.Register<ISpeedService>(resolver =>
                new SpeedService(
                    gameplaySettings.InitialSpeed,
                    gameplaySettings.SpeedIncreaseStep,
                    gameplaySettings.MaxSpeed),
            Lifetime.Singleton);

        builder.Register<IRunScoreService, RunScoreService>(Lifetime.Singleton);
        builder.Register<IPlayerEffectsService, PlayerEffectsService>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<MapGenerator>();
        builder.RegisterComponentInHierarchy<Player>();
        builder.RegisterComponentInHierarchy<CheckObstacle>();
        builder.RegisterComponentInHierarchy<RunHudView>();
        builder.RegisterComponentInHierarchy<GameOverView>();
        builder.RegisterComponentInHierarchy<GameStateUiRouter>();
        builder.RegisterComponentInHierarchy<RunnerAudioController>();

        builder.RegisterEntryPoint<RunnerGameFlowEntryPoint>(Lifetime.Singleton);
        builder.RegisterEntryPoint<RunScoreEntryPoint>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerEffectsEntryPoint>(Lifetime.Singleton);
        builder.RegisterEntryPoint<RunResultCommitEntryPoint>(Lifetime.Singleton);
        builder.RegisterEntryPoint<SpeedProgressionEntryPoint>(Lifetime.Singleton);
    }
}
