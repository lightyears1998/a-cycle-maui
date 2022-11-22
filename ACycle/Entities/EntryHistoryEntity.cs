using ACycle.AppServices;
using SQLite;

namespace ACycle.Entities
{
    [Table("entry_history")]
    public class EntryHistoryEntity
    {
        [PrimaryKey]
        [Column("id")]
        public int Id { get; set; }

        [Column("entryUuid")]
        public Guid EntryUuid { get; set; } = Guid.NewGuid();

        [Column("entryUpdatedAt")]
        public string EntryUpdatedAt { get; set; } = DateTime.Now.ToString();

        [Column("entryUpdatedBy")]
        public Guid EntryUpdatedBy { get; set; } = Guid.Empty;

        [Column("createdAt")]
        public DateTime createdAt { get; set; }
    }
}
