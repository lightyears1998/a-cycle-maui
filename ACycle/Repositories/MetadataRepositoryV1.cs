using ACycle.Services;
using ACycle.Entities;

namespace ACycle.Repositories
{
    public class MetadataRepositoryV1 : Repository<MetadataV1>
    {
        private readonly IDatabaseService _databaseService;

        public MetadataRepositoryV1(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<string?> GetMetadataAsync(string Key)
        {
            var query = _databaseService.MainDatabase.Table<MetadataV1>().Where(meta => meta.Key == Key);
            var result = await query.FirstOrDefaultAsync();
            return result?.Value;
        }

        public async Task SaveMetadataAsync(string Key, string Value)
        {
            await _databaseService.MainDatabase.InsertOrReplaceAsync(new MetadataV1 { Key = Key, Value = Value });
        }
    }
}
