using Nakama.Leaderboard;
using UnityEngine;

namespace Nakama.Game
{
    public class NakamaGameModule : MonoBehaviour
    {
        #region serialized variables
        [SerializeField] private SocialLeaderboardController _LeaderboardController;
        #endregion

        #region private variables
        private ISocialLeaderboard _iLeaderboard;
        #endregion

        #region public methods
        public void SubmitScore(LeaderboardSubmitScoreData scoreData)
        {
            _iLeaderboard.SubmitScore(scoreData);
        }
        #endregion

        #region mono methods
        private void Start()
        {
            _LeaderboardController.InitializeMono(this);
            _iLeaderboard = _LeaderboardController;
        }
        #endregion
    }
}