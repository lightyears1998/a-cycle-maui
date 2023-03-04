using SQLite;

namespace ACycle.Entities
{
    [Table("user_synchronization_endpoint")]
    public class SynchronizationEndpointV1
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int? Id { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { set; get; } = true;

        [Column("host")]
        public string Host { get; set; } = string.Empty;

        [Column("path")]
        public string Path { set; get; } = string.Empty;

        [Column("http_port")]
        public int HttpPort { set; get; } = 443;

        [Column("ws_port")]
        public int WsPort { set; get; } = 44;

        [Column("use_tls")]
        public bool UseTLS { set; get; } = true;

        [Column("username")]
        public string Username { set; get; } = "guest";

        [Column("password")]
        public string Password { set; get; } = string.Empty;
    }
}
