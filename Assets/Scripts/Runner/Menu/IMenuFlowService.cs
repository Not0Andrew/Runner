using Cysharp.Threading.Tasks;

namespace Runner.Menu
{
    public interface IMenuFlowService
    {
        UniTask StartGameAsync();
        void ExitGame();
    }
}
