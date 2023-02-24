namespace ACycle.Models.User
{
    public class SynchronizationConfig
    {
        public string Host = string.Empty;

        public string Path = "api";

        public int HttpPort = 443;

        public int WsPort = 44;

        public string Username = "guest";

        public string Password = "pa$$w0rd";

        public bool UseTLS = true;

        public bool IsEnabled = true;
    }
}
