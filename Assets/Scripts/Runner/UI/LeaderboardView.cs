using System.Collections.Generic;
using Runner.Core;
using Runner.Progression;
using TMPro;
using UnityEngine;

namespace Runner.UI
{
    public sealed class LeaderboardView : MonoBehaviour
    {
        [SerializeField] private Transform entriesRoot;
        [SerializeField] private LeaderboardEntryView entryPrefab;
        [SerializeField] private TMP_Text emptyStateText;
        [SerializeField] private int maxEntries = 10;

        private ILeaderboardService _leaderboardService;
        private readonly List<LeaderboardEntryView> _entries = new List<LeaderboardEntryView>();

        private void OnEnable()
        {
            _leaderboardService ??= RunnerRuntimeContext.LeaderboardService;
            Refresh();
        }

        public void Refresh()
        {
            if (_leaderboardService == null)
                return;

            var top = _leaderboardService.GetTop(maxEntries);
            Rebuild(top);
        }

        private void Rebuild(IReadOnlyList<ScoreRecord> top)
        {
            ClearEntries();

            bool hasEntries = top != null && top.Count > 0;
            if (emptyStateText != null)
                emptyStateText.gameObject.SetActive(!hasEntries);

            if (!hasEntries || entriesRoot == null || entryPrefab == null)
                return;

            for (int i = 0; i < top.Count; i++)
            {
                var record = top[i];
                var entry = Instantiate(entryPrefab, entriesRoot);
                entry.SetData(i + 1, record.Name, record.Coins);
                _entries.Add(entry);
            }
        }

        private void ClearEntries()
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                if (_entries[i] != null)
                    Destroy(_entries[i].gameObject);
            }

            _entries.Clear();
        }
    }
}
