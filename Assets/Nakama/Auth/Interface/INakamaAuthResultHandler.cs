namespace Nakama.Auth
{
    public interface INakamaAuthResultHandler
    {
        public void OnConnectedToServer();
        public void OnAuthenticatedDevice();
    }
}