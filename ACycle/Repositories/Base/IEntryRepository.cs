namespace ACycle.Repositories
{
    public interface IEntryRepository<T> : IRepository<T>
        where T : Entities.Entry, new()
    {
        Task<List<T>> FindAllAsync();

        Task<T?> FindByUuidAsync(Guid uuid);

        Task HardDeleteAsync(T entry);

        Task InsertAsync(T entry);

        Task UpdateAsync(T entry);

        Task RemoveAsync(T entry);

        Task SaveIfFresherAsync(IEnumerable<T> entries);
    }
}
