using SQLite;

namespace ACycle.Entities
{
    [Table("peer_node")]
    public class PeerNodeEntity
    {
        [PrimaryKey]
        [Column("uuid")]
        public Guid Uuid { get; set; } = Guid.NewGuid();

        public string HistoryCursor = string.Empty;

        [Column("updatedAt")]
        public DateTime UpdatedAt { set; get; } = DateTime.Now;
    }
}
