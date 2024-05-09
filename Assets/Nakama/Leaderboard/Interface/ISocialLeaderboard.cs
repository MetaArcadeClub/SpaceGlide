using System;
using System.Collections.Generic;

namespace Nakama.Leaderboard
{
    public interface ISocialLeaderboard
    {
        public void GetLeaderboardRecordsList(string leaderboardId, Action<IEnumerable<IApiLeaderboardRecord>> callback);
        public void SubmitScore(LeaderboardSubmitScoreData scoreData);
    }
}