using ACycle.Repositories;
using CommunityToolkit.Diagnostics;

namespace ACycle.Services
{
    public class MetadataService : Service, IMetadataService
    {
        private readonly MetadataRepository _metadataRepository;

        public MetadataService(MetadataRepository metadataRepository)
        {
            _metadataRepository = metadataRepository;
        }

        public async Task<string> GetMetadataAsync(string key, string? defaultValue)
        {
            string? value = await _metadataRepository.GetMetadataAsync(key);
            if (value == null)
            {
                if (defaultValue == null)
                {
                    ThrowHelper.ThrowArgumentNullException(nameof(defaultValue));
                }
                value = defaultValue;
                await _metadataRepository.SaveMetadataAsync(key, defaultValue);
            }
            return value;
        }

        public async Task<Guid> GetNodeUuidAsync()
        {
            return Guid.Parse(await GetMetadataAsync("NODE_UUID", defaultValue: Guid.NewGuid().ToString()));
        }
    }
}
