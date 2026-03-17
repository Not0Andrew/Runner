using System.Collections.Generic;

namespace Runner.Progression
{
    public interface ILeaderboardService
    {
        IReadOnlyList<ScoreRecord> GetTop(int limit = 20);
        void SaveRecord(string playerName, int coins);
    }
}
