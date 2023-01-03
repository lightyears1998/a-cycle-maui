namespace ACycle.Services
{
    public class NavigationService : INavigationService
    {
        public Task NavigateToAsync(string route, IDictionary<string, object>? parameters)
        {
            return parameters != null ? Shell.Current.GoToAsync(route, parameters) : Shell.Current.GoToAsync(route);
        }

        public Task PopAsync() => Shell.Current.GoToAsync("..");
    }
}
