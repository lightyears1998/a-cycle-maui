using SQLite;

namespace ACycle.Entities
{
    [Table("entry")]
    public class Entry : ISoftRemovableEntity
    {
        [PrimaryKey]
        [Column("uuid")]
        public Guid Uuid { set; get; } = Guid.NewGuid();

        [Column("content_type")]
        public string ContentType { set; get; } = string.Empty;

        [Column("content")]
        public string Content { set; get; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { set; get; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { set; get; } = DateTime.UtcNow;

        [Column("updated_by")]
        public Guid UpdatedBy { set; get; } = Guid.Empty;

        [Column("removed_at")]
        public DateTime? RemovedAt { set; get; }
    }
}
