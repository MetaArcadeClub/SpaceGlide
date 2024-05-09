using System;

namespace NakamaAdmin.Data
{
    [Serializable]
    public class CreateLeaderboardResultData
    {
        public bool Success;
        public string LeaderboardId;

        public CreateLeaderboardResultData(bool success, string leaderboardId)
        {
            Success = success;
            LeaderboardId = leaderboardId;
        }
    }
}