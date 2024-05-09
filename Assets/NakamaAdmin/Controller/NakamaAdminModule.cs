using System;
using System.Threading.Tasks;
using Nakama;
using UnityEngine;
using nk = NakamaAdmin.Constants.NakamaAdminConstants;

namespace NakamaAdmin.Controller
{
    public class NakamaAdminModule : MonoBehaviour
    {
        #region serialized variables
        [SerializeField] private bool _CreateLeaderboard;
        #endregion

        #region private variables
        private IClient _client;
        private ISession _session;
        private bool _lastCreateLeaderboardState;
        #endregion

        #region mono methods methods
        private void Start()
        {
        }

        private void OnValidate()
        {
            if (_CreateLeaderboard == _lastCreateLeaderboardState) 
                return;
            
            _lastCreateLeaderboardState = _CreateLeaderboard;
            if (_CreateLeaderboard)
                AuthenticateMasterClient(result =>
                {
                    if (result.Username == string.Empty)
                    {
                        Debug.LogError("Failed to auth.");
                        return;
                    }
                        
                    Debug.Log("Authentication complete");
                    CreateLeaderboard(response =>
                    {
                        Debug.Log($"RPC Response: {response.Payload}");
                    });
                });
        }
        #endregion

        #region private methods
        private void AuthenticateMasterClient(Action<ISession> callback)
        {
            Debug.Log("Called authentication");
            _client = new Client(nk.scheme, nk.host, nk.port, nk.serverKey);
            AuthenticateCustomAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Debug.LogError("Couldn't authenticate");
                    return;
                }

                callback?.Invoke(t.Result);
            });
        }

        private void CreateLeaderboard(Action<IApiRpc> callback)
        {
            ExecuteRPC(nk.createLeaderboardRPC).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Debug.LogError("Couldn't create leaderboard");
                    return;
                }

                callback.Invoke(t.Result);
            });
        }
        
        private Task<ISession> AuthenticateCustomAsync()
        {
            return _client.AuthenticateCustomAsync(nk.masterClientUID, null, true);
        }

        private Task<IApiRpc> ExecuteRPC(string rpc)
        {
            return _client.RpcAsync(_session, rpc);
        }
        #endregion
    }
}