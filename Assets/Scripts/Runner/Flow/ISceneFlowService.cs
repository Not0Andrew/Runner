using Cysharp.Threading.Tasks;

namespace Runner.Flow
{
    public interface ISceneFlowService
    {
        UniTask LoadBootstrapAsync();
        UniTask LoadMenuAsync();
        UniTask LoadGameAsync();
        UniTask ReloadCurrentAsync();
    }
}
