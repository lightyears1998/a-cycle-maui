using SQLite;

namespace ACycle.Entities
{
    [Table("node_peer")]
    public class NodePeerV1
    {
        [PrimaryKey]
        [Column("uuid")]
        public Guid Uuid { get; set; } = Guid.NewGuid();

        [Column("history_cursor")]
        public string HistoryCursor { set; get; } = string.Empty;

        [Column("updated_at")]
        public DateTime UpdatedAt { set; get; } = DateTime.UtcNow;
    }
}
