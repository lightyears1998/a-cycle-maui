using ACycle.Services;

namespace ACycleMaui.Services
{
    public interface INavigationService : IService
    {
        public Task NavigatToAsync(string route);
    }
}
