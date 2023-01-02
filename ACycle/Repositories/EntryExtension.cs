namespace ACycle.Repositories
{
    public static class EntryExtension
    {
        public static EntryMetadata GetMetadata(this Entities.Entry entry)
        {
            return new EntryMetadata
            {
                Uuid = entry.Uuid,
                CreatedAt = entry.CreatedAt,
                UpdatedAt = entry.UpdatedAt,
                UpdatedBy = entry.UpdatedBy,
                RemovedAt = entry.RemovedAt,
            };
        }
    }
}
