using SQLite;

namespace ACycle.Entities
{
    [Table("entry")]
    public class EntryEntity : ISoftRemovableEntity
    {
        [PrimaryKey]
        [Column("uuid")]
        public Guid Uuid { set; get; } = Guid.NewGuid();

        [Column("contentType")]
        public string ContentType { set; get; } = string.Empty;

        [Column("content")]
        public string Content { set; get; } = string.Empty;

        [Column("createdAt")]
        public DateTime CreatedAt { set; get; } = DateTime.Now;

        [Column("updatedAt")]
        public DateTime UpdatedAt { set; get; } = DateTime.Now;

        [Column("updatedBy")]
        public Guid UpdatedBy { set; get; } = Guid.Empty;

        [Column("removedAt")]
        public DateTime? RemovedAt { set; get; }
    }
}
