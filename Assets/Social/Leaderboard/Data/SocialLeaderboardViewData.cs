using Nakama.Leaderboard;
using UiViewController.Data;

namespace Social.Leaderboard
{
    public class SocialLeaderboardViewData: BaseUiViewData
    {
        public ISocialLeaderboard SocialLeaderboard;
        public MainThreadDispatcher Dispatcher;

        public SocialLeaderboardViewData(ISocialLeaderboard socialLeaderboard, MainThreadDispatcher dispatcher)
        {
            SocialLeaderboard = socialLeaderboard;
            Dispatcher = dispatcher;
        }
    }
}