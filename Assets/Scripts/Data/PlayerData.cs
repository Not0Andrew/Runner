using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class PlayerData
{
    private CharacterSkins _selectedCharacterSkin;
    private MazeSkins _selectedMazeSkin;

    private List<CharacterSkins> _openCharacterSkins;
    private List<MazeSkins> _openMazeSkins;

    private int _money;
    private List<ScoreRecord> _scoreRecords;
    private int _selectedBonusStyle;
    private List<int> _openBonusStyles;

    public PlayerData()
    {
        _money = 30000;

        _selectedCharacterSkin = CharacterSkins.Bear;
        _selectedMazeSkin = MazeSkins.China;

        _openCharacterSkins = new List<CharacterSkins>() { _selectedCharacterSkin };
        _openMazeSkins = new List<MazeSkins>() { _selectedMazeSkin };
        _scoreRecords = new List<ScoreRecord>();
        _selectedBonusStyle = 0;
        _openBonusStyles = new List<int> { _selectedBonusStyle };
    }

    [JsonConstructor]
    public PlayerData(int money, CharacterSkins selectedCharacterSkin, MazeSkins selectedMazeSkin,
        List<CharacterSkins> openCharacterSkins, List<MazeSkins> openMazeSkins, List<ScoreRecord> scoreRecords = null,
        int selectedBonusStyle = 0, List<int> openBonusStyles = null)
    {
        Money = money;

        _selectedCharacterSkin = selectedCharacterSkin;
        _selectedMazeSkin = selectedMazeSkin;

        _openCharacterSkins = new List<CharacterSkins>(openCharacterSkins);
        _openMazeSkins = new List<MazeSkins>(openMazeSkins);
        _scoreRecords = scoreRecords != null ? new List<ScoreRecord>(scoreRecords) : new List<ScoreRecord>();
        _openBonusStyles = openBonusStyles != null ? new List<int>(openBonusStyles) : new List<int> { 0 };
        _selectedBonusStyle = _openBonusStyles.Contains(selectedBonusStyle) ? selectedBonusStyle : _openBonusStyles[0];
    }

    public int Money
    {
        get => _money;

        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            _money = value;
        }
    }

    public CharacterSkins SelectedCharacterSkin
    {
        get => _selectedCharacterSkin;
        set
        {
            if (_openCharacterSkins.Contains(value) == false)
                throw new ArgumentException(nameof(value));

            _selectedCharacterSkin = value;
        }
    }

    public MazeSkins SelectedMazeSkin
    {
        get => _selectedMazeSkin;
        set
        {
            if (_openMazeSkins.Contains(value) == false)
                throw new ArgumentException(nameof(value));

            _selectedMazeSkin = value;
        }
    }

    public IEnumerable<CharacterSkins> OpenCharacterSkins => _openCharacterSkins;

    public IEnumerable<MazeSkins> OpenMazeSkins => _openMazeSkins;
    public IEnumerable<ScoreRecord> ScoreRecords => _scoreRecords;
    public IEnumerable<int> OpenBonusStyles => _openBonusStyles;

    public int SelectedBonusStyle
    {
        get => _selectedBonusStyle;
        set
        {
            if (_openBonusStyles.Contains(value) == false)
                throw new ArgumentException(nameof(value));

            _selectedBonusStyle = value;
        }
    }

    public void OpenCharacterSkin(CharacterSkins skin)
    {
        if(_openCharacterSkins.Contains(skin))
            throw new ArgumentException(nameof(skin));

        _openCharacterSkins.Add(skin);
    }

    public void OpenMazeSkin(MazeSkins skin)
    {
        if (_openMazeSkins.Contains(skin))
            throw new ArgumentException(nameof(skin));

        _openMazeSkins.Add(skin);
    }

    public void AddScoreRecord(string name, int coins)
    {
        _scoreRecords.Add(new ScoreRecord(name, coins));
    }

    public IReadOnlyList<ScoreRecord> GetTopScoreRecords(int limit)
    {
        if (limit <= 0)
            return Array.Empty<ScoreRecord>();

        return _scoreRecords
            .OrderByDescending(record => record.Coins)
            .ThenByDescending(record => record.TimestampUtcTicks)
            .Take(limit)
            .ToList();
    }

    public void OpenBonusStyle(int style)
    {
        if (style < 0)
            throw new ArgumentOutOfRangeException(nameof(style));

        if (_openBonusStyles.Contains(style))
            throw new ArgumentException(nameof(style));

        _openBonusStyles.Add(style);
    }
}
