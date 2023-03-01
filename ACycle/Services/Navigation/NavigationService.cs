namespace ACycle.Services
{
    public class NavigationService : Service, INavigationService
    {
        public Task NavigateToAsync(string route, IDictionary<string, object>? parameters)
        {
            return parameters != null ? Shell.Current.GoToAsync(route, parameters) : Shell.Current.GoToAsync(route);
        }

        public Task GoBackAsync() => Shell.Current.GoToAsync("..");
    }
}
