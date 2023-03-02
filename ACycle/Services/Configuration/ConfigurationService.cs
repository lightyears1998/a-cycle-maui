using System.Globalization;

namespace ACycle.Services
{
    public class ConfigurationService : Service, IConfigurationService
    {
        private readonly IMetadataService _metadataService;

        public Guid NodeUuid { set; get; }

        public CultureInfo SystemCultureInfo { get; } = CultureInfo.CurrentCulture;

        public ConfigurationService(IMetadataService metadataService)
        {
            _metadataService = metadataService;
        }

        public override async Task InitializeAsync()
        {
            NodeUuid = await GetNodeUuid();
        }

        private async Task<Guid> GetNodeUuid()
        {
            return Guid.Parse(await _metadataService.GetMetadataAsync(MetadataKeys.NODE_UUID, defaultValue: Guid.NewGuid().ToString()));
        }

        protected static class MetadataKeys
        {
            public const string NODE_UUID = "NODE_UUID";
        }
    }
}
