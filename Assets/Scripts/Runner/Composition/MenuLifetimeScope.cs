using Runner.UI;
using VContainer;
using VContainer.Unity;

public sealed class MenuLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<MainMenuView>();
        builder.RegisterComponentInHierarchy<LeaderboardView>();
        builder.RegisterComponentInHierarchy<ShopBootstrap>();
    }
}
