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
            NodeUuid = await _metadataService.GetNodeUuidAsync();
        }
    }
}
