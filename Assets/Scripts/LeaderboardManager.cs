using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    private NakamaConnection nakamaConnection;

    // Fetches leaderboard from Nakama
    public async Task FetchLeaderboard()
    {
        // var topScores = await nakamaConnection.GetTopScores();
        // You can then sort or process the scores here if necessary
    }

    // If you still want a local leaderboard (e.g., for caching), you can keep it
    // but update it with Nakama data.
    public List<GameManager.ScoreEntry> leaderboard = new List<GameManager.ScoreEntry>();
}
