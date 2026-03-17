using TMPro;
using UnityEngine;

namespace Runner.UI
{
    public sealed class LeaderboardEntryView : MonoBehaviour
    {
        [SerializeField] private TMP_Text placeText;
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text scoreText;

        public void SetData(int place, string playerName, int score)
        {
            if (placeText != null)
                placeText.text = place.ToString();

            if (playerNameText != null)
                playerNameText.text = string.IsNullOrWhiteSpace(playerName) ? "Player" : playerName;

            if (scoreText != null)
                scoreText.text = score.ToString();
        }
    }
}
