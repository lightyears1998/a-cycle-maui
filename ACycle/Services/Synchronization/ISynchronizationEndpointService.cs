using ACycle.Models;

namespace ACycle.Services
{
    public interface ISynchronizationEndpointService
    {
        event EventHandler? SynchronizationEndpointChanged;

        Task<IList<SynchronizationEndpoint>> LoadAsync();

        Task RemoveAsync(SynchronizationEndpoint config);

        Task SaveAsync(SynchronizationEndpoint config);
    }
}
