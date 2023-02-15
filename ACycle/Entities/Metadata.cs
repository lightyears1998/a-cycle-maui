using SQLite;

namespace ACycle.Entities
{
    [Table("metadata")]
    public class Metadata
    {
        [PrimaryKey]
        [Column("key")]
        public string Key { get; set; } = string.Empty;

        [Column("value")]
        public string Value { get; set; } = string.Empty;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
