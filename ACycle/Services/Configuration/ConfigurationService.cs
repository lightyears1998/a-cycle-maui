using ACycle.Repositories;

namespace ACycle.Services
{
    public class ConfigurationService : Service, IConfigurationService
    {
        public Guid NodeUuid { set; get; }

        private readonly MetadataRepository _metadataRepository;

        public ConfigurationService(MetadataRepository metadataRepository)
        {
            _metadataRepository = metadataRepository;
        }

        public override async Task InitializeAsync()
        {
            NodeUuid = Guid.Parse(await GetOrSetMetadata("NODE_UUID", Guid.NewGuid().ToString()));
        }

        private async Task<string> GetOrSetMetadata(string key, string defaultValue)
        {
            string? value = await _metadataRepository.GetMetadataAsync(key);
            if (value == null)
            {
                value = defaultValue;
                await _metadataRepository.SaveMetadataAsync(key, defaultValue);
            }
            return value;
        }
    }
}
