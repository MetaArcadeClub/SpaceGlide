using System;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using System.Threading.Tasks;
using System.Linq;
using Social.Leaderboard;
using UnityMainThreadDispatcher;

public class NakamaConnection : MonoBehaviour
{
    #region public variables
    public IClient Client { get; private set; }
    public SocialLeaderboardModule _LeaderboardModule;
    #endregion

    #region private variables
    private ISession Session;
    private ISocket Socket;
    private MainDispatcher _dispatcher;

    private string scheme = "http";
    // private string host = "127.0.0.1";
    private string host = "86.123.116.78";
    private int port = 7350;
    private string serverKey = "defaultkey";

    private const string SessionPrefName = "nakama.session";
    private const string DeviceIdentifierPrefName = "nakama.deviceUniqueIdentifier";
    #endregion

    private void Awake()
    {
        _dispatcher = MainDispatcher.Instance();
    }

    private async void Start()
    {
        // if (!Socket.IsConnected)
        // await Connect();
        
        var metadata = new Dictionary<string, string>
        {
            { "testKey", "testValue" }
        };
        // SubmitScore(1000, "YourPlayerName");
    }

    public async Task Connect()
    {
        Client = new Nakama.Client(scheme, host, port, serverKey, UnityWebRequestAdapter.Instance);

        var authToken = PlayerPrefs.GetString(SessionPrefName);
        if (!string.IsNullOrEmpty(authToken))
        {
            var session = Nakama.Session.Restore(authToken);
            if (!session.IsExpired)
            {
                Session = session;
            }
        }

        if (Session == null)
        {
            string deviceId = PlayerPrefs.HasKey(DeviceIdentifierPrefName)
                ? PlayerPrefs.GetString(DeviceIdentifierPrefName)
                : SystemInfo.deviceUniqueIdentifier;

            if (deviceId == SystemInfo.unsupportedIdentifier)
            {
                deviceId = System.Guid.NewGuid().ToString();
            }

            PlayerPrefs.SetString(DeviceIdentifierPrefName, deviceId);

            try
            {
                // Session = await Client.AuthenticateDeviceAsync(deviceId);
                // PlayerPrefs.SetString(SessionPrefName, Session.AuthToken);
                await AuthenticateDevice(deviceId).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Debug.LogError("Couldn't authenticate with device.");
                    }

                    if (!t.IsCompletedSuccessfully)
                        return;
                    
                    Debug.Log("Authenticated with device");
                    _dispatcher.Enqueue(() =>
                    {
                        Session = t.Result;
                        PlayerPrefs.SetString(SessionPrefName, Session.AuthToken);
                    });
                });
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to authenticate: {e}");
                return;
            }
        }

        Socket = Client.NewSocket();

        try
        {
            await Socket.ConnectAsync(Session, true).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Debug.LogError("Couldn't connect to nakama");
                }

                if (!t.IsCompletedSuccessfully) 
                    return;
                
                Debug.Log("Successfully connected to nakama");
            });
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to connect socket: {e}");
        }
    }

    public async Task SubmitScore(long score, string playerName, string leaderboardId = "weekly_top_200")
    {
        // Check if session is expired
        if (Session.IsExpired)
        {
            await Connect(); // Re-authenticate
        }

        // Create a custom payload with playerName and deviceId
        var payloadData = new
        {
            playerName = playerName,
            deviceId = SystemInfo.deviceUniqueIdentifier
        };
        var payloadJson = JsonUtility.ToJson(payloadData);

        try
        {
            var record = await Client.WriteLeaderboardRecordAsync(Session, leaderboardId, score, 0, payloadJson);
            Debug.Log($"New record for '{record.Username}' score '{record.Score}' with payload '{record.Metadata}'");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to submit score: {e}");
        }
    }

    public void GetTopScores(int limit, Action<IApiLeaderboardRecordList> callback)
    {
        var leaderboardId = "weekly_top_200";
        try
        {
            var result =  Client.ListLeaderboardRecordsAsync(Session, leaderboardId).ContinueWith(t =>
            {
                foreach (var r in t.Result.Records)
                {
                    System.Console.WriteLine("Record for '{0}' score '{1}'", r.Username, r.Score);
                }
            });
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to get top scores: {e}");
        }
    }

    public async Task<List<IApiLeaderboardRecord>> GetScoresAroundUser(string userId, int limit = 100)
    {
        var leaderboardId = "weekly_top_200";
        try
        {
            var records = await Client.ListLeaderboardRecordsAroundOwnerAsync(Session, leaderboardId, userId, limit);
            return records.Records.ToList();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to get scores around user: {e}");
            return null;
        }
    }

    #region private methods

    private Task<ISession> AuthenticateDevice(string deviceId)
    {
        return Client.AuthenticateDeviceAsync(deviceId);
    }
    #endregion
}
