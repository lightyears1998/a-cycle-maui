using SQLite;

namespace ACycle.Entities
{
    public class Entry
    {
        [PrimaryKey]
        [Column("uuid")]
        public Guid Uuid { get; set; } = Guid.NewGuid();

        [Column("created_at")]
        public DateTime? CreatedAt { set; get; }

        public bool IsCreated => CreatedAt.HasValue;

        [Column("created_by")]
        public Guid? CreatedBy { set; get; }

        [Column("updated_at")]
        public DateTime UpdatedAt { set; get; } = DateTime.Now;

        [Column("updated_by")]
        public Guid UpdatedBy { set; get; } = Guid.Empty;

        [Column("removed_at")]
        public DateTime? RemovedAt { get; set; }

        public bool IsRemoved => RemovedAt.HasValue;
    }
}
