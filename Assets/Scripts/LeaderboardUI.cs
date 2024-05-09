using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class LeaderboardUI : MonoBehaviour
{
    public LeaderboardManager leaderboardManager;
    public Text leaderboardText;

    private void Start()
    {
        RefreshLeaderboard();
    }

    public async Task RefreshLeaderboard()
    {
        await leaderboardManager.FetchLeaderboard();
        DisplayLeaderboard();
    }

    public void DisplayLeaderboard()
    {
        string displayText = "";
        foreach (var entry in leaderboardManager.leaderboard)
        {
            displayText += entry.playerName + ": " + entry.score + "\n";
        }

        leaderboardText.text = displayText;
    }
}
