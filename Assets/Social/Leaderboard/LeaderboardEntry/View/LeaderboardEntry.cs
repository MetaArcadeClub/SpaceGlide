using TMPro;
using UnityEngine;

namespace Social.Leaderboard
{
    public class LeaderboardEntry : MonoBehaviour
    {
        #region serialized variables
        [SerializeField] private TextMeshProUGUI _PlayerName;
        [SerializeField] private TextMeshProUGUI _PlayerScore;
        #endregion
        
        #region private variables
        private string _playerName;
        private int _playerScore;
        #endregion

        #region public methods
        public void SetData(string playerName, int playerScore)
        {
            _playerName = playerName;
            _playerScore = playerScore;
            
            Show();
        }
        #endregion

        #region private methods

        private void Show()
        {
            _PlayerName.text = _playerName;
            _PlayerScore.text = _playerScore.ToString();
        }
        #endregion
    }
}