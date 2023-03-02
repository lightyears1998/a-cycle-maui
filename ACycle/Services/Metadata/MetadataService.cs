using ACycle.Repositories;
using CommunityToolkit.Diagnostics;
using System.Numerics;

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

        public async Task<bool> GetBoolMetadataAsync(string key, bool? defaultValue)
        {
            string stringValue = defaultValue.HasValue ? await GetMetadataAsync(key, defaultValue.ToString()) : await GetMetadataAsync(key, null);
            return bool.Parse(stringValue);
        }

        public async Task<TNumber> GetNumberMetadataAsync<TNumber>(string key, TNumber? defaultValue) where TNumber : INumber<TNumber>
        {
            string stringValue = defaultValue != null ? await GetMetadataAsync(key, defaultValue.ToString()) : await GetMetadataAsync(key, null);
            return TNumber.Parse(stringValue, System.Globalization.NumberStyles.None, null);
        }

        public async Task SetMetadataAsync(string key, string value)
        {
            Guard.IsNotNull(key);
            Guard.IsNotNull(value);

            await _metadataRepository.SaveMetadataAsync(key, value);
        }

        public async Task SetBoolMetadataAsync(string key, bool value)
        {
            await _metadataRepository.SaveMetadataAsync(key, value.ToString());
        }

        public async Task SetNumberMetadataAsync<TNumber>(string key, TNumber value) where TNumber : INumber<TNumber>
        {
            await _metadataRepository.SaveMetadataAsync(key, value.ToString()!);
        }
    }
}
