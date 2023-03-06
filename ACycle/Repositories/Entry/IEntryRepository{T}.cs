using ACycle.Repositories.Entry;

namespace ACycle.Repositories
{
    public interface IEntryRepository<T> : IRepository<T>
        where T : Entities.Entry, new()
    {
        Task HardDeleteAsync(T entry);

        Task InsertAsync(T entry);

        Task UpdateAsync(T entry);

        Task RemoveAsync(T entry);

        Task SaveIfFresherAsync(IEnumerable<T> entries);

        Task<List<T>> FindAllAsync(bool includeRemoved = false);

        Task<T?> FindByUuidAsync(Guid uuid, bool includeRemoved = true);

        Task<List<EntryMetadata>> GetAllMetadataAsync();
    }
}
