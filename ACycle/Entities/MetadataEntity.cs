using SQLite;

namespace ACycle.Entities
{
    [Table("metadata")]
    public class MetadataEntity
    {
        [PrimaryKey]
        [Column("key")]
        public string Key { get; set; } = string.Empty;

        [Column("value")]
        public string Value { get; set; } = string.Empty;

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
