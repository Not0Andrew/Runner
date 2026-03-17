using Runner.Config;
using Runner.EntryPoints;
using Runner.Flow;
using Runner.Progression;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class BootstrapLifetimeScope : LifetimeScope
{
    [SerializeField] private SceneFlowSettings _sceneFlowSettings = new SceneFlowSettings();

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_sceneFlowSettings);

        builder.Register<IPersistentData, PersistentData>(Lifetime.Singleton);
        builder.Register<IDataProvider, DataLocalProvider>(Lifetime.Singleton);
        builder.Register<Wallet>(Lifetime.Singleton);
        builder.Register<ILeaderboardService, LeaderboardService>(Lifetime.Singleton);
        builder.Register<ISceneFlowService, SceneFlowService>(Lifetime.Singleton);

        builder.RegisterEntryPoint<PersistentDataEntryPoint>(Lifetime.Singleton);
        builder.RegisterEntryPoint<BootstrapStartupEntryPoint>(Lifetime.Singleton);
    }
}
