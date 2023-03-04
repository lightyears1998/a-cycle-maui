using ACycle.Models;

namespace ACycle.Services
{
    public class SynchronizationService : Service, ISynchronizationService
    {
        private readonly IMetadataService _metadataService;
        private readonly ISynchronizationEndpointService _endpointService;

        public bool SynchronizationEnabled { get; protected set; }

        public ObservableCollectionEx<SynchronizationEndpoint> SynchronizationEndpoints { get; } = new();

        public SynchronizationService(IMetadataService metadataService, ISynchronizationEndpointService endpointService)
        {
            _metadataService = metadataService;
            _endpointService = endpointService;
            _endpointService.SynchronizationEndpointChanged += OnEndpointChanged;
        }

        public override async Task InitializeAsync()
        {
            SynchronizationEnabled = await _metadataService.GetBoolMetadataAsync(MetadataKeys.SYNCHRONIZATION_ENABLED, false);
            await LoadEndpointsAsync();
        }

        public async Task SetSynchronizationEnabledAsync(bool value)
        {
            await _metadataService.SetBoolMetadataAsync(MetadataKeys.SYNCHRONIZATION_ENABLED, value);
            SynchronizationEnabled = value;
        }

        protected async void OnEndpointChanged(object? sender, EventArgs args)
        {
            await LoadEndpointsAsync();
        }

        protected async Task LoadEndpointsAsync()
        {
            SynchronizationEndpoints.Reload(await _endpointService.LoadAsync());
        }

        protected static class MetadataKeys
        {
            public const string SYNCHRONIZATION_ENABLED = "SYNCHRONIZATION_ENABLED";
        }
    }
}
