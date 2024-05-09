namespace Nakama.Auth
{
    public interface INakamaAuth
    {
        public void ConnectToServer();

        public void AuthenticateDevice();
    }
}