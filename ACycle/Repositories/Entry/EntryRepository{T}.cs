using ACycle.Services;
using ACycle.Services.Database;

namespace ACycle.Repositories
{
    public class EntryRepository<T> : Repository<T>, IEntryRepository<T>
        where T : Entities.Entry, new()
    {
        private readonly IDatabaseConnectionWrapper _connection;

        public EntryRepository(IDatabaseService databaseService)
        {
            _connection = databaseService;
        }

        public EntryRepository(IDatabaseConnectionWrapper connectionWrapper)
        {
            _connection = connectionWrapper;
        }

        public async Task InsertAsync(T entry)
        {
            await _connection.MainDatabase.InsertAsync(entry);
            OnEntityCreated(entry);
        }

        public async Task UpdateAsync(T entry)
        {
            await _connection.MainDatabase.UpdateAsync(entry);
            OnEntityUpdated(entry);
        }

        public async Task RemoveAsync(T entry)
        {
            await _connection.MainDatabase.UpdateAsync(entry);
            OnEntityRemoved(entry);
        }

        public async Task HardDeleteAsync(T entry)
        {
            await _connection.MainDatabase.DeleteAsync(entry);
            OnEntityRemoved(entry);
        }

        public async Task SaveIfFresherAsync(IEnumerable<T> entries)
        {
            foreach (var incomingEntry in entries)
            {
                var existsEntry = await FindByUuidAsync(incomingEntry.Uuid);

                if (existsEntry == null)
                {
                    await InsertAsync(incomingEntry);
                    continue;
                }

                if (incomingEntry.UpdatedAt > existsEntry.UpdatedAt)
                {
                    await UpdateAsync(incomingEntry);
                    continue;
                }
            }
        }

        public async Task<List<T>> FindAllAsync()
        {
            var query = _connection.MainDatabase.Table<T>()
                .Where(entry => entry.RemovedAt == null);
            return await query.ToListAsync();
        }

        public async Task<T?> FindByUuidAsync(Guid uuid)
        {
            var query = _connection.MainDatabase.Table<T>()
                .Where(entry => entry.RemovedAt == null && entry.Uuid == uuid);
            return await query.FirstOrDefaultAsync();
        }
    }
}
