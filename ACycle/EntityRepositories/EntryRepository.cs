using ACycle.AppServices;
using ACycle.Entities;

namespace ACycle.EntityRepositories
{
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

        private void UpdateTimestamp(EntryEntity entry)
        {
            entry.UpdatedAt = DateTime.UtcNow;
            entry.UpdatedBy = _configurationService.NodeUuid;
        }

        public async Task InsertAsync(EntryEntity entry, bool updateTimestamp = true)
        {
            if (updateTimestamp) UpdateTimestamp(entry);

            await _databaseService.MainDatabase.InsertAsync(entry);
        }

        public async Task UpdateAsync(EntryEntity entry, bool updateTimestamp = true)
        {
            if (updateTimestamp) UpdateTimestamp(entry);

            await _databaseService.MainDatabase.UpdateAsync(entry);
        }

        public async Task DeleteAsync(EntryEntity entry)
        {
            await _databaseService.MainDatabase.DeleteAsync(entry);
        }

        public async Task<List<EntryEntity>> FindAllAsync(string contentType)
        {
            var query = _databaseService.MainDatabase.Table<EntryEntity>().Where(entity => entity.ContentType == contentType);
            return await query.ToListAsync();
        }
    }
}
