using System;
using System.Collections.Generic;
using Nakama;
using Nakama.Leaderboard;
using UiViewController;
using UnityEngine;

namespace Social.Leaderboard
{
    public class SocialLeaderboardView : BaseUiViewController<SocialLeaderboardViewRefs>
    {
        #region serialized variables
        [SerializeField] private string _LeaderboardId;
        #endregion

        #region private variables
        private ISocialLeaderboard _socialLeaderboard;
        private List<IApiLeaderboardRecord> _leaderboardRecords;
        private List<LeaderboardEntry> _leaderboardEntriesList;
        private MainThreadDispatcher _mainDispatcher;
        #endregion

        #region public methods
        public override void Show(object data)
        {
            base.Show(data);
            var viewData = (SocialLeaderboardViewData) data;
            _socialLeaderboard = viewData.SocialLeaderboard;
            _mainDispatcher = viewData.Dispatcher;
            
            gameObject.SetActive(true);
            FetchLeaderboardRecords(_LeaderboardId);
        }
        #endregion

        #region private methods
        private void FetchLeaderboardRecords(string leaderboardId)
        {
            _socialLeaderboard.GetLeaderboardRecordsList(leaderboardId, result =>
            {
                if (result.Equals(null))
                    return;

                _leaderboardRecords = new List<IApiLeaderboardRecord>();
                foreach (var record in result)
                {
                    _leaderboardRecords.Add(record);
                }

                _mainDispatcher.Enqueue(() => PopulateLeaderboard(_leaderboardRecords));
            });
        }

        private void PopulateLeaderboard(List<IApiLeaderboardRecord> recordsList)
        {
            _leaderboardEntriesList ??= new List<LeaderboardEntry>();

            while (_leaderboardEntriesList.Count < recordsList.Count)
            {
                var entry = Instantiate(_ViewRefs.LeaderboardEntry, _ViewRefs.LeaderboardEntryParent);
                entry.gameObject.SetActive(false);
                _leaderboardEntriesList.Add(entry);
            }
            
            for (int i = 0; i < recordsList.Count; i++)
            {
                var entry = _leaderboardEntriesList[i];
                var record = recordsList[i];
                
                entry.SetData(record.Username, Int32.Parse(record.Score));
                entry.gameObject.SetActive(true);
            }

            for (int i = recordsList.Count; i < _leaderboardEntriesList.Count; i++)
            {
                _leaderboardEntriesList[i].gameObject.SetActive(false);
            }
        }
        #endregion
    }
}