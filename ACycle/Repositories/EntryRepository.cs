using ACycle.Services;

namespace ACycle.Repositories
{
    using Entry = Entities.Entry;

    public class EntryRepository
    {
        private readonly IDatabaseService _databaseService;

        private readonly IConfigurationService _configurationService;

        public EntryRepository(
            IDatabaseService databaseService,
            IConfigurationService configurationService)
        {
            _databaseService = databaseService;
            _configurationService = configurationService;
        }

        private void UpdateTimestamp(Entry entry)
        {
            entry.UpdatedAt = DateTime.UtcNow;
            entry.UpdatedBy = _configurationService.NodeUuid;
        }

        public async Task InsertAsync(Entry entry, bool updateTimestamp = true)
        {
            if (updateTimestamp) UpdateTimestamp(entry);

            await _databaseService.MainDatabase.InsertAsync(entry);
        }

        public async Task UpdateAsync(Entry entry, bool updateTimestamp = true)
        {
            if (updateTimestamp) UpdateTimestamp(entry);

            await _databaseService.MainDatabase.UpdateAsync(entry);
        }

        public async Task RemoveAsync(Entry entry)
        {
            entry.RemovedAt = DateTime.UtcNow;

            await _databaseService.MainDatabase.UpdateAsync(entry);
        }

        public async Task ForceDelete(Entry entry)
        {
            await _databaseService.MainDatabase.DeleteAsync(entry);
        }

        public async Task<List<Entry>> FindAllAsync(string contentType)
        {
            var query = _databaseService.MainDatabase.Table<Entry>().Where(entry => entry.ContentType == contentType);
            return await query.ToListAsync();
        }

        public async Task<Entry?> FindByUuid(Guid uuid)
        {
            var query = _databaseService.MainDatabase.Table<Entry>().Where(entry => entry.Uuid == uuid);
            return await query.FirstAsync();
        }
    }
}
