using System.Security.Cryptography;
using System.Text;

namespace ACycle.Models
{
    public record class SynchronizationEndpoint
    {
        public long? Id { get; set; }

        public bool IsEnabled { get; set; } = true;

        public string Host { get; set; } = string.Empty;

        public string Path { get; set; } = "api";

        public int HttpPort { get; set; } = 443;

        public int WsPort { get; set; } = 443;

        public bool UseTLS { get; set; } = true;

        public string Username { get; set; } = "guest";

        public string Password { get; set; } = "pa$$w0rd";

        public string PasswordSha256
        {
            get
            {
                var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(Password));
                return Convert.ToHexString(bytes).ToLower();
            }
        }

        public string BriefDescription => $"{Username}@{Host}/{Path} {HttpPort}:{WsPort} (Enabled: {IsEnabled})";

        public Uri HttpUri
        {
            get
            {
                var schema = UseTLS ? "https" : "http";
                return new($"{schema}://{Host.TrimEnd('/')}:{HttpPort}/{Path.Trim('/')}/");
            }
        }

        public Uri WsUri
        {
            get
            {
                var schema = UseTLS ? "wss" : "ws";
                return new($"{schema}://{Host.TrimEnd('/')}:{WsPort}/{Path.Trim('/')}/socket");
            }
        }
    }
}
