using ACycle.Services;

namespace ACycle.ViewModels
{
    public class LandingViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IConfigurationService _configurationService;

        public LandingViewModel(IDatabaseService databaseService, IConfigurationService configurationService)
        {
            _databaseService = databaseService;
            _configurationService = configurationService;
        }

        public override async Task InitializeAsync()
        {
            await InitializeServices();
            NavigateToAppShell();
        }

        private async Task InitializeServices()
        {
            await _databaseService.InitializeAsync();
            await _configurationService.InitializeAsync();
        }

        private static void NavigateToAppShell()
        {
            Application.Current!.MainPage = new AppShell();
        }
    }
}
