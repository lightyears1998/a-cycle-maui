using SQLite;

namespace ACycle.Entities
{
    [Table("peer_node")]
    public class PeerNodeEntity : IEntity
    {
        [PrimaryKey]
        [Column("uuid")]
        public Guid Uuid { get; set; } = Guid.NewGuid();

        [Column("historyCursor")]
        public string HistoryCursor { set; get; } = string.Empty;

        [Column("updatedAt")]
        public DateTime UpdatedAt { set; get; } = DateTime.UtcNow;
    }
}
