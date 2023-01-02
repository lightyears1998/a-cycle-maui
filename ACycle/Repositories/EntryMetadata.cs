namespace ACycle.Repositories
{
    public class EntryMetadata
    {
        public Guid Uuid { set; get; } = Guid.NewGuid();

        public DateTime CreatedAt { set; get; } = DateTime.UtcNow;

        public DateTime UpdatedAt { set; get; } = DateTime.UtcNow;

        public Guid UpdatedBy { set; get; } = Guid.Empty;

        public DateTime? RemovedAt { set; get; }
    }
}
