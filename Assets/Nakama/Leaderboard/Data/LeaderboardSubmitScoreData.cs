using System;

namespace Nakama.Leaderboard
{
    [Serializable]
    public class LeaderboardSubmitScoreData
    {
        public string LeaderboardId;
        public long Score;
    }
}