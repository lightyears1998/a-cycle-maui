using ACycle.Services;

namespace ACycle.Repositories
{
    public class EntryRepository<T> : Repository<T>, IEntryRepository<T>
        where T : Entities.Entry, new()
    {
        private readonly IDatabaseService _databaseService;

        public EntryRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task InsertAsync(T entry)
        {
            await _databaseService.MainDatabase.InsertAsync(entry);
            OnEntityCreated(entry);
        }

        public async Task UpdateAsync(T entry)
        {
            await _databaseService.MainDatabase.UpdateAsync(entry);
            OnEntityUpdated(entry);
        }

        public async Task RemoveAsync(T entry)
        {
            await _databaseService.MainDatabase.UpdateAsync(entry);
            OnEntityRemoved(entry);
        }

        public async Task HardDeleteAsync(T entry)
        {
            await _databaseService.MainDatabase.DeleteAsync(entry);
            OnEntityRemoved(entry);
        }

        public async Task<List<T>> FindAllAsync()
        {
            var query = _databaseService.MainDatabase.Table<T>()
                .Where(entry => entry.RemovedAt == null);
            return await query.ToListAsync();
        }

        public async Task<T> FindByUuid(Guid uuid)
        {
            var query = _databaseService.MainDatabase.Table<T>()
                .Where(entry => entry.RemovedAt == null && entry.Uuid == uuid);
            return await query.FirstAsync();
        }
    }
}
