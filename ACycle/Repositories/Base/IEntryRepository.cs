namespace ACycle.Repositories
{
    public interface IEntryRepository<T> : IRepository<T>
        where T : Entities.Entry, new()
    {
        Task<List<T>> FindAllAsync();

        Task<T> FindByUuid(Guid uuid);

        Task HardDeleteAsync(T entry);

        Task InsertAsync(T entry);

        Task UpdateAsync(T entry);

        Task RemoveAsync(T entry);
    }
}
