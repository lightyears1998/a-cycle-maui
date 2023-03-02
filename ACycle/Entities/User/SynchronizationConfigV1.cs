using SQLite;

namespace ACycle.Entities
{
    [Table("user_synchronization_config")]
    public class SynchronizationConfigV1
    {
        [PrimaryKey]
        [Column("id")]
        public long Id { get; set; }

        [Column("host")]
        public string Host { get; set; } = string.Empty;

        [Column("path")]
        public string Path { set; get; } = string.Empty;

        [Column("http_port")]
        public int HttpPort { set; get; } = 443;

        [Column("ws_port")]
        public int WsPort { set; get; } = 44;

        [Column("username")]
        public string Username { set; get; } = "guest";

        [Column("password_sha256")]
        public string PasswordSha256 { set; get; } = string.Empty;

        [Column("use_tls")]
        public bool UseTLS { set; get; } = true;

        [Column("is_enabled")]
        public bool IsEnabled { set; get; } = true;
    }
}
