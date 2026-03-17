using VContainer.Unity;

namespace Runner.EntryPoints
{
    public sealed class PersistentDataEntryPoint : IStartable
    {
        private readonly IPersistentData _persistentData;
        private readonly IDataProvider _dataProvider;

        public PersistentDataEntryPoint(IPersistentData persistentData, IDataProvider dataProvider)
        {
            _persistentData = persistentData;
            _dataProvider = dataProvider;
        }

        public void Start()
        {
            if (_dataProvider.TryLoad() == false)
                _persistentData.PlayerData = new PlayerData();
        }
    }
}
