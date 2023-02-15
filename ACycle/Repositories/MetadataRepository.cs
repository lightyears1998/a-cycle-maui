using ACycle.Services;
using ACycle.Entities;

namespace ACycle.Repositories
{
    public class MetadataRepository :
        Repository<Metadata>
    {
        private readonly IDatabaseService _databaseService;

        public MetadataRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<string?> GetMetadataAsync(string Key)
        {
            var query = _databaseService.MainDatabase.Table<Metadata>().Where(meta => meta.Key == Key);
            var result = await query.FirstOrDefaultAsync();
            return result?.Value;
        }

        public async Task SaveMetadataAsync(string Key, string Value)
        {
            await _databaseService.MainDatabase.InsertOrReplaceAsync(new Metadata { Key = Key, Value = Value });
        }
    }
}
