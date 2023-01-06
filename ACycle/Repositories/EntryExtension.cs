namespace ACycle.Repositories
{
    public static class EntryExtension
    {
        public static EntryMetadata GetMetadata(this Entities.Entry entry)
        {
            return new EntryMetadata
            {
                CreatedAt = entry.CreatedAt,
                CreatedBy = entry.CreatedBy,
                UpdatedAt = entry.UpdatedAt,
                UpdatedBy = entry.UpdatedBy,
                RemovedAt = entry.RemovedAt,
            };
        }
    }
}
