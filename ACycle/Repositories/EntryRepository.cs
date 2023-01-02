using ACycle.Services;

namespace ACycle.Repositories
{
    using Entry = Entities.Entry;

    public class EntryRepository
    {
        private readonly IDatabaseService _databaseService;

        public EntryRepository(
            IDatabaseService databaseService
        )
        {
            _databaseService = databaseService;
        }

        public async Task InsertAsync(Entry entry)
        {
            await _databaseService.MainDatabase.InsertAsync(entry);
        }

        public async Task UpdateAsync(Entry entry)
        {
            await _databaseService.MainDatabase.UpdateAsync(entry);
        }

        public async Task HardDeleteAsync(Entry entry)
        {
            await _databaseService.MainDatabase.DeleteAsync(entry);
        }

        public async Task<List<Entry>> FindAllAsync(string contentType)
        {
            var query = _databaseService.MainDatabase.Table<Entry>()
                .Where(entry => entry.RemovedAt == null && entry.ContentType == contentType);
            return await query.ToListAsync();
        }

        public async Task<Entry?> FindByUuid(Guid uuid)
        {
            var query = _databaseService.MainDatabase.Table<Entry>()
                .Where(entry => entry.RemovedAt == null && entry.Uuid == uuid);
            return await query.FirstAsync();
        }
    }
}
