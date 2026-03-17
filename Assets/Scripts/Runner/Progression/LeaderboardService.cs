using System.Collections.Generic;

namespace Runner.Progression
{
    public sealed class LeaderboardService : ILeaderboardService
    {
        private readonly IPersistentData _persistentData;
        private readonly IDataProvider _dataProvider;

        public LeaderboardService(IPersistentData persistentData, IDataProvider dataProvider)
        {
            _persistentData = persistentData;
            _dataProvider = dataProvider;
        }

        public IReadOnlyList<ScoreRecord> GetTop(int limit = 20)
        {
            return _persistentData.PlayerData.GetTopScoreRecords(limit);
        }

        public void SaveRecord(string playerName, int coins)
        {
            _persistentData.PlayerData.AddScoreRecord(playerName, coins);
            _dataProvider.Save();
        }
    }
}
