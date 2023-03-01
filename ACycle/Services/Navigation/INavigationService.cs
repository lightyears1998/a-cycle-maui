namespace ACycle.Services
{
    public interface INavigationService : IService
    {
        public Task NavigateToAsync(string route, IDictionary<string, object>? parameters = null);

        public Task GoBackAsync();
    }
}
