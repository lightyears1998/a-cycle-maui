using ACycle.Resources.Strings;

namespace ACycle.Services
{
    public class NavigationService : Service, INavigationService
    {
        private readonly IDialogService _dialogService;

        public NavigationService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public Task NavigateToAsync(string route, IDictionary<string, object>? parameters)
        {
            return parameters != null ? Shell.Current.GoToAsync(route, parameters) : Shell.Current.GoToAsync(route);
        }

        public Task GoBackAsync() => Shell.Current.GoToAsync("..");

        public async Task ConfirmForLeaveAsync(string? title = null, string? message = null)
        {
            title ??= AppStrings.Text_ConfirmForLeave;
            message ??= AppStrings.Text_UnsavedModificationsWillBeLost;

            var shouldGoBack = await _dialogService.RequestAsync(title, message);

            if (shouldGoBack)
            {
                await GoBackAsync();
            }
        }
    }
}
