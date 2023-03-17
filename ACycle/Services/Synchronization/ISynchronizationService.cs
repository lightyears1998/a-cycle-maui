using ACycle.Models;

namespace ACycle.Services
{
    public interface ISynchronizationService
    {
        bool SynchronizationEnabled { get; }

        string SynchronizationStatus { get; }

        bool SynchronizationEnabledWhenWiFiIsConnected { get; }

        bool SynchronizationEnabledWhenEthernetIsConnected { get; }

        bool SynchronizationEnabledWhenCellularIsConnected { get; }

        ObservableCollectionEx<SynchronizationEndpoint> SynchronizationEndpoints { get; }

        Task InitializeAsync();

        Task SetSynchronizationEnabledAsync(bool value);

        Task SetSynchronizationEnabledWhenCellularIsConnectedAsync(bool value);

        Task SetSynchronizationEnabledWhenEthernetIsConnectedAsync(bool value);

        Task SetSynchronizationEnabledWhenWiFiIsConnectedAsync(bool value);

        Task SyncAsync();
    }
}
