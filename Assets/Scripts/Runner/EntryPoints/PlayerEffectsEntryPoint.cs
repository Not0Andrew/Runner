using Runner.Gameplay;
using VContainer.Unity;

namespace Runner.EntryPoints
{
    public sealed class PlayerEffectsEntryPoint : IStartable
    {
        private readonly IPlayerEffectsService _effectsService;

        public PlayerEffectsEntryPoint(IPlayerEffectsService effectsService)
        {
            _effectsService = effectsService;
        }

        public void Start()
        {
            _effectsService.Reset();
        }
    }
}
