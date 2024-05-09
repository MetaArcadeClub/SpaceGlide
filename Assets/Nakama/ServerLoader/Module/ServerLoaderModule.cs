using Nakama.Auth;
using UnityEngine;

namespace Nakama.ServerLoader
{
    public class ServerLoaderModule : MonoBehaviour
    {
        #region public variables
        public ServerLoaderController ServerController;
        public NakamaAuthController AuthController;
        #endregion

        #region private variables
        private INakamaAuth _iAuth;
        #endregion

        #region mono methods
        private void Awake()
        {
            var serverLoaderData = new ServerLoaderData {MonoBehaviour = this};
            ServerController.Initialize(serverLoaderData);
            _iAuth = AuthController;
        }

        private void Start()
        {
            _iAuth.AuthenticateDevice();
        }
        #endregion
    }
}