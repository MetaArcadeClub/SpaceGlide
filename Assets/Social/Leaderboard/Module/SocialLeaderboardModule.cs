using Nakama.Leaderboard;
using UnityEngine;

namespace Social.Leaderboard
{
    public class SocialLeaderboardModule : MonoBehaviour
    {
        #region serialized variables
        [SerializeField] private SocialLeaderboardView _View;
        [SerializeField] private SocialLeaderboardController _leaderboardController;
        #endregion
       
        #region private variables
        private MainThreadDispatcher _mainDispatcher;
        #endregion

        #region public methods
        public void ShowLeaderboard()
        {
            var viewData = new SocialLeaderboardViewData(_leaderboardController, _mainDispatcher);
            _View.Show(viewData);
        }
        #endregion

        #region mono methods
        private void Awake()
        {
            _mainDispatcher = MainThreadDispatcher.Instance();
        }
        #endregion
    }
}