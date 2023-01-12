using SQLite;

namespace ACycle.Entities
{
    public class Entry : ISoftRemovableEntity
    {
        public Guid Uuid { set; get; } = Guid.Empty;

        public string ContentType { set; get; } = string.Empty;

        public string Content { set; get; } = string.Empty;

        public DateTime CreatedAt { set; get; } = DateTime.UtcNow;

        public Guid CreatedBy { set; get; } = Guid.Empty;

        public DateTime UpdatedAt { set; get; } = DateTime.UtcNow;

        public Guid UpdatedBy { set; get; } = Guid.Empty;

        public DateTime? RemovedAt { set; get; }
    }
}
