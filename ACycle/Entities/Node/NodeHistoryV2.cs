using SQLite;

namespace ACycle.Entities
{
    [Table("node_history")]
    public class NodeHistoryV2
    {
        [PrimaryKey]
        [Column("id")]
        public int Id { get; set; }

        [Column("entry_uuid")]
        public Guid EntryUuid { get; set; } = Guid.NewGuid();

        [Column("entry_updated_at")]
        public DateTime EntryUpdatedAt { get; set; } = DateTime.Now;

        [Column("entry_updated_by")]
        public Guid EntryUpdatedBy { get; set; } = Guid.Empty;
    }
}
