using UnityEngine;
using nk = Nakama.Constants.NakamaConstants;

namespace Nakama.ServerLoader
{
    [CreateAssetMenu(menuName = MenuName, fileName = FilePath + FileName)]
    public class ServerLoaderController : ScriptableObject
    {
        #region consts
        private const string MenuName = "Nakama/ServerLoader/ServerLoader";
        private const string FilePath = "Assets/Nakama/ServerLoader/ScriptableObject/Resources/";
        private const string FileName = "ServerLoaderController";
        #endregion

        #region public variables
        public MonoBehaviour Mono;
        public IClient Client;
        public ISession Session;
        public ISocket Socket;
        #endregion

        #region private variables
        
        #if UNITY_WEBGL && !UNITY_EDITOR
         ISocketAdapter adapter = new JsWebSocketAdapter();
        #else
        ISocketAdapter adapter = new WebSocketAdapter();
        #endif

        #endregion

        #region public methods
        public void Initialize(ServerLoaderData serverLoaderData)
        {
            Mono = serverLoaderData.MonoBehaviour;
            InitClient();
            RestoreSession();
        }
        #endregion

        #region private methods
        private void InitClient()
        {
            Client ??= new Client(nk.SchemeHttps, nk.RomanianRigHostAddress, nk.Port, nk.ServerKey, UnityWebRequestAdapter.Instance);
            Socket ??= Nakama.Socket.From(Client, adapter);
        }
        
        private void RestoreSession()
        {
            var authToken = PlayerPrefs.GetString(nk.SessionPref);
            if (!string.IsNullOrEmpty(authToken))
            {
                var session = Nakama.Session.Restore(authToken);
                if (session.IsExpired)
                {
                    Debug.LogError("Session has expired");
                    return;
                }

                Session = session;
            }
        }
        #endregion
    }
}