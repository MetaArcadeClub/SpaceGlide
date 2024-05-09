using Nakama.ServerLoader;
using UnityEngine;

namespace Nakama
{
    [CreateAssetMenu(menuName = MenuName, fileName = FilePath + FileName)]
    public class BaseSocialController : ScriptableObject
    {
        #region consts
        private const string MenuName = "Nakama/BaseSocial/Controller";
        private const string FilePath = "Assets/Nakama/BaseSocial/Resources/";
        private const string FileName = "BaseSocialController";
        #endregion
        
        #region public variables
        public ServerLoaderController Server;
        #endregion

        #region properties
        protected MonoBehaviour _Mono { get => Server.Mono; set => Server.Mono = value; }
        protected IClient _Client { get => Server.Client; set => Server.Client = value; }
        protected ISocket _Socket { get => Server.Socket; set => Server.Socket = value; }
        protected ISession _Session { get => Server.Session; set => Server.Session = value; }
        #endregion

        #region public methods
        public void InitializeMono(MonoBehaviour monoBehaviour)
        {
            Server.Mono = monoBehaviour;
        }
        #endregion
    }
}