using ACycle.Models;

namespace ACycle.Services
{
    public interface ISynchronizationService
    {
        bool SynchronizationEnabled { get; }

        ObservableCollectionEx<SynchronizationEndpoint> SynchronizationEndpoints { get; }

        Task InitializeAsync();

        Task SetSynchronizationEnabled(bool value);
    }
}
