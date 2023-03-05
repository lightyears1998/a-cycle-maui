using ACycle.Models;

namespace ACycle.Services
{
    public interface ISynchronizationService
    {
        bool SynchronizationEnabled { get; }

        string SynchronizationStatus { get; }

        ObservableCollectionEx<SynchronizationEndpoint> SynchronizationEndpoints { get; }

        Task InitializeAsync();

        Task SetSynchronizationEnabledAsync(bool value);

        Task SyncAsync();
    }
}
