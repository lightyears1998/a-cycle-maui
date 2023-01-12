using SQLite;

namespace ACycle.Entities
{
    [Table("node_metadata")]
    public class NodeMetadataV1
    {
        [PrimaryKey]
        [Column("key")]
        public string Key { get; set; } = string.Empty;

        [Column("value")]
        public string Value { get; set; } = string.Empty;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
