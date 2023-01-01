namespace ACycleMaui.Services
{
    public class NavigationService : INavigationService
    {
        public async Task NavigatToAsync(string route)
        {
            await Shell.Current.GoToAsync(route);
        }
    }
}
