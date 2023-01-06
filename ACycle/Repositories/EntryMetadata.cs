namespace ACycle.Repositories
{
    public class EntryMetadata
    {
        public DateTime? CreatedAt { set; get; }

        public Guid? CreatedBy { set; get; }

        public DateTime UpdatedAt { set; get; } = DateTime.UtcNow;

        public Guid UpdatedBy { set; get; } = Guid.Empty;

        public DateTime? RemovedAt { set; get; }

        public bool IsCreated
        {
            get => CreatedAt.HasValue;
        }

        public bool IsRemoved
        {
            get => RemovedAt.HasValue;
        }
    }
}
