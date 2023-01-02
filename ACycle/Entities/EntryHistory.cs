using ACycle.Services;
using SQLite;

namespace ACycle.Entities
{
    [Table("entry_history")]
    public class EntryHistory : IEntity
    {
        [PrimaryKey]
        [Column("id")]
        public int Id { get; set; }

        [Column("entry_uuid")]
        public Guid EntryUuid { get; set; } = Guid.NewGuid();

        [Column("entry_updated_at")]
        public DateTime EntryUpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("entry_updated_by")]
        public Guid EntryUpdatedBy { get; set; } = Guid.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
