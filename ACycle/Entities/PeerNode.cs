using SQLite;

namespace ACycle.Entities
{
    [Table("peer_node")]
    public class PeerNode : IEntity
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
