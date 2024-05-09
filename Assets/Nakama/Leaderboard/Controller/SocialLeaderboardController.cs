using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nakama.CustomYield;
using UnityEngine;

namespace Nakama.Leaderboard
{
    [CreateAssetMenu(menuName = MenuName, fileName = FilePath + FileName)]
    public class SocialLeaderboardController : BaseSocialController, ISocialLeaderboard
    {
        #region consts
        private const string MenuName = "Nakama/Leaderboard/Controller";
        private const string FilePath = "Assets/Nakama/Leaderboard/Data/Resources/";
        private const string FileName = "SocialLeaderboardController";
        #endregion
        
        #region private variables
        #region coroutines
        private Coroutine _fetchLeaderboardCoroutine;
        private Coroutine _submitScoreCoroutine;
        #endregion
        #endregion

        #region explicit implementations
        void ISocialLeaderboard.GetLeaderboardRecordsList(string leaderboardId, Action<IEnumerable<IApiLeaderboardRecord>> callback)
        {
            StartFetchRecordsCoroutine(leaderboardId, callback);
        }

        void ISocialLeaderboard.SubmitScore(LeaderboardSubmitScoreData scoreData)
        {
            StartSubmitScoreCoroutine(scoreData);
        }
        #endregion

        #region private methods
        private void StartFetchRecordsCoroutine(string leaderboardId, Action<IEnumerable<IApiLeaderboardRecord>> callback)
        {
            if (_fetchLeaderboardCoroutine != null)
            {
                _Mono.StopCoroutine(_fetchLeaderboardCoroutine);
                _fetchLeaderboardCoroutine = null;
            }

            _fetchLeaderboardCoroutine = _Mono.StartCoroutine(FetchLeaderboardRecordsCoroutine(leaderboardId, callback));
        }

        private void StartSubmitScoreCoroutine(LeaderboardSubmitScoreData scoreData)
        {
            if (_submitScoreCoroutine != null)
            {
                _Mono.StopCoroutine(_submitScoreCoroutine);
                _submitScoreCoroutine = null;
            }

            _submitScoreCoroutine = _Mono.StartCoroutine(SubmitScoreCoroutine(scoreData));
        }

        private IEnumerator SubmitScoreCoroutine(LeaderboardSubmitScoreData scoreData)
        {
            var submitScoreTask = SubmitScore(scoreData);
            yield return new WaitForTask(submitScoreTask);
            Debug.LogError($"Submit Score Status: {submitScoreTask.Status}");

            if (submitScoreTask.IsFaulted)
            {
                Debug.LogError($"Submit Score Failed: {submitScoreTask.Exception}");
            }

            if (submitScoreTask.IsCompletedSuccessfully)
            {
                Debug.LogError($"Successfully submitted score: {scoreData.Score} on leaderboard: {scoreData.LeaderboardId}");
                // var record = submitScoreTask.Result;
            }
        }

        private IEnumerator FetchLeaderboardRecordsCoroutine(string leaderboardId, Action<IEnumerable<IApiLeaderboardRecord>> callback)
        {
            var fetchRecordsTask = FetchLeaderboardRecords(leaderboardId);
            yield return new WaitForTask(fetchRecordsTask);
            Debug.LogError($"Fetched Records Status: {fetchRecordsTask.Status}");

            if (fetchRecordsTask.IsFaulted)
            {
                Debug.LogError($"Error fetching records: {fetchRecordsTask.Exception}");
                callback.Invoke(null);
            }

            if (fetchRecordsTask.IsCompletedSuccessfully)
            {
                Debug.LogError($"Fetched Records Successfully: {fetchRecordsTask.Id}");
                var records = fetchRecordsTask.Result.Records;
                callback.Invoke(records);
            }
        }

        private Task<IApiLeaderboardRecordList> FetchLeaderboardRecords(string leaderboardId)
        {
            return _Client.ListLeaderboardRecordsAsync(_Session, leaderboardId, null, null, 10);
        }

        private Task<IApiLeaderboardRecord> SubmitScore(LeaderboardSubmitScoreData scoreData)
        {
            var leaderboardId = scoreData.LeaderboardId;
            var score = scoreData.Score;
            return _Client.WriteLeaderboardRecordAsync(_Session, leaderboardId, score);
        }
        #endregion
    }
}