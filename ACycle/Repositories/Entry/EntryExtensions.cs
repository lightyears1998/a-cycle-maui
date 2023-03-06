using ACycle.Entities;

namespace ACycle.Repositories.Entry
{
    public static class EntryExtensions
    {
        public static EntryMetadata GetMetadata<TEntry>(this TEntry entry)
            where TEntry : Entities.Entry
        {
            return new EntryMetadata
            {
                Uuid = entry.Uuid,
                CreatedAt = entry.CreatedAt,
                UpdatedAt = entry.UpdatedAt,
                UpdatedBy = entry.UpdatedBy,
            };
        }

        public static bool IsFresherThan(this IEntryComparable entry, IEntryComparable other)
        {
            return entry.UpdatedAt > other.UpdatedAt;
        }
    }
}
