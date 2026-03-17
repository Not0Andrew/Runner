using System;

[Serializable]
public sealed class ScoreRecord
{
    public string Name;
    public int Coins;
    public long TimestampUtcTicks;

    public ScoreRecord(string name, int coins)
    {
        Name = string.IsNullOrWhiteSpace(name) ? "Player" : name.Trim();
        Coins = Math.Max(0, coins);
        TimestampUtcTicks = DateTime.UtcNow.Ticks;
    }
}
