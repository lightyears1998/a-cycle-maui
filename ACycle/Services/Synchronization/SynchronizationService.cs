using ACycle.Models;
using ACycle.Repositories;
using ACycle.Services.Synchronization;
using Microsoft.Extensions.Logging;

namespace ACycle.Services
{
    public class SynchronizationService : Service, ISynchronizationService
    {
        private readonly ILogger _serviceLogger;
        private readonly ILogger _workerLogger;

        private readonly IConfigurationService _configurationService;
        private readonly IEntryRepository _entryRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMetadataService _metadataService;
        private readonly ISynchronizationEndpointService _endpointService;

        private IDispatcherTimer? _syncCountdownTimer;

        public bool SynchronizationEnabled { get; protected set; }

        public TimeSpan SynchronizationInterval { get; } = TimeSpan.FromMinutes(10);

        public string SynchronizationStatus { get; protected set; } = string.Empty;

        public ObservableCollectionEx<SynchronizationEndpoint> SynchronizationEndpoints { get; } = new();

        public SynchronizationService(
            ILogger<SynchronizationService> serviceLogger,
            ILogger<SynchronizationWorker> workerLogger,
            IConfigurationService configurationService,
            IEntryRepository entryRepository,
            IServiceProvider serviceProvider,
            IMetadataService metadataService,
            ISynchronizationEndpointService endpointService)
        {
            _serviceLogger = serviceLogger;
            _workerLogger = workerLogger;

            _configurationService = configurationService;
            _entryRepository = entryRepository;
            _serviceProvider = serviceProvider;
            _metadataService = metadataService;
            _endpointService = endpointService;

            _endpointService.SynchronizationEndpointChanged += OnEndpointChanged;
        }

        public override async Task InitializeAsync()
        {
            SynchronizationEnabled = await _metadataService.GetBoolMetadataAsync(MetadataKeys.SYNCHRONIZATION_ENABLED, false).ConfigureAwait(false);
            await LoadEndpointsAsync().ConfigureAwait(false);

            AdjustTimer();

            _serviceLogger.LogInformation($"{nameof(SynchronizationService)} initialized.");
        }

        public async Task SetSynchronizationEnabledAsync(bool value)
        {
            await _metadataService.SetBoolMetadataAsync(MetadataKeys.SYNCHRONIZATION_ENABLED, value);
            SynchronizationEnabled = value;

            AdjustTimer();
        }

        private void AdjustTimer()
        {
            if (SynchronizationEnabled)
            {
                SetupTimer();
            }
            else
            {
                ClearSyncCountdownTimer();
            }
        }

        private void SetupTimer()
        {
            ClearSyncCountdownTimer();

            _syncCountdownTimer = Application.Current!.Dispatcher.CreateTimer();
            _syncCountdownTimer.IsRepeating = true;
            _syncCountdownTimer.Interval = SynchronizationInterval;
            _syncCountdownTimer.Tick += OnSyncCountdownTimerTick;
            _syncCountdownTimer.Start();
        }

        private void ClearSyncCountdownTimer()
        {
            if (_syncCountdownTimer != null)
            {
                _syncCountdownTimer.Stop();
                _syncCountdownTimer = null;
            }
        }

        private void OnSyncCountdownTimerTick(object? sender, EventArgs args)
        {
            _serviceLogger.LogInformation($"{nameof(OnSyncCountdownTimerTick)} started.");

            SyncAsync().ContinueWith((_) =>
            {
                _serviceLogger.LogInformation($"{nameof(OnSyncCountdownTimerTick)} completed.");
            });
        }

        protected async void OnEndpointChanged(object? sender, EventArgs args)
        {
            await LoadEndpointsAsync();
        }

        protected async Task LoadEndpointsAsync()
        {
            SynchronizationEndpoints.Reload(await _endpointService.LoadAsync());
        }

        public async Task SyncAsync()
        {
            try
            {
                foreach (var endpoint in SynchronizationEndpoints.Where(endpoint => endpoint.IsEnabled))
                {
                    await SyncAtEndpointAsync(endpoint).ConfigureAwait(false);
                }
            }
            catch (Exception ex) when (ex is SynchronizationException || ex is not null)
            {
                SynchronizationStatus = ex.ToString();
                return;
            }

            SynchronizationStatus = "Sync completed.";
        }

        public async Task SyncAtEndpointAsync(SynchronizationEndpoint endpoint)
        {
            var worker = new SynchronizationWorker(
                endpoint,
                _workerLogger,
                _configurationService,
                _entryRepository);

            await worker.SyncAsync().ConfigureAwait(false);
        }

        protected static class MetadataKeys
        {
            public const string SYNCHRONIZATION_ENABLED = "SYNCHRONIZATION_ENABLED";
        }
    }
}
