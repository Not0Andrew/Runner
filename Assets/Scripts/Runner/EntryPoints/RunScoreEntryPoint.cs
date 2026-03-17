using Runner.Gameplay;
using VContainer.Unity;

namespace Runner.EntryPoints
{
    public sealed class RunScoreEntryPoint : IStartable
    {
        private readonly IRunScoreService _runScoreService;

        public RunScoreEntryPoint(IRunScoreService runScoreService)
        {
            _runScoreService = runScoreService;
        }

        public void Start()
        {
            _runScoreService.Reset();
        }
    }
}
