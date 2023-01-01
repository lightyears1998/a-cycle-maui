using ACycle.Services;
using SQLite;

namespace ACycle.Entities
{
    [Table("entry_history")]
    public class EntryHistoryEntity : IEntity
    {
        [PrimaryKey]
        [Column("id")]
        public int Id { get; set; }

        [Column("entryUuid")]
        public Guid EntryUuid { get; set; } = Guid.NewGuid();

        [Column("entryUpdatedAt")]
        public DateTime EntryUpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("entryUpdatedBy")]
        public Guid EntryUpdatedBy { get; set; } = Guid.Empty;

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
