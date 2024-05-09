using UnityEngine;
using UnityEngine.UI;
using Nakama;
using System.Collections.Generic;

public class LeaderboardUIManager : MonoBehaviour
{
    public GameObject leaderboardPanel; // Reference to the UI panel or scroll view for the leaderboard
    public GameObject leaderboardEntryPrefab; // Reference to the leaderboard entry prefab

    [SerializeField ]private NakamaConnection _NakamaConnection; // Reference to the NakamaConnection script

    private IApiLeaderboardRecordList _recordList;
    private void Start()
    {
        // _NakamaConnection = FindObjectOfType<NakamaConnection>();
    }

    public async void DisplayLeaderboard()
    {
        // Fetch the leaderboard data
        
        _NakamaConnection.GetTopScores(200, result =>
        {
            _recordList = result;
            var records = _recordList.Records;
            foreach (var record in records)
            {
                GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardPanel.transform);

                // Get the text fields in the prefab
                Text playerNameText = entry.transform.Find("PlayerNameText").GetComponent<Text>();
                Text playerScoreText = entry.transform.Find("PlayerScoreText").GetComponent<Text>();

                // Update the text fields with the player's name and score
                playerNameText.text = record.Username;
                playerScoreText.text = record.Score;
            }
        });
    }
    
    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
    }

    public void HideLeaderboard()
    {
        leaderboardPanel.SetActive(false);
    }

}
