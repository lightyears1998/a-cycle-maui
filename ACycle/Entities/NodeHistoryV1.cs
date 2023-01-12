using SQLite;

namespace ACycle.Entities
{
    [Table("node_history")]
    public class NodeHistoryV1
    {
        [PrimaryKey]
        [Column("id")]
        public int Id { get; set; }

        [Column("model_uuid")]
        public Guid ModelUuid { get; set; } = Guid.NewGuid();

        [Column("model_updated_at")]
        public DateTime ModelUpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("model_updated_by")]
        public Guid ModelUpdatedBy { get; set; } = Guid.Empty;
    }
}
