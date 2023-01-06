using ACycle.Repositories;
using ACycle.Services;
using Microsoft.Maui.Platform;

namespace ACycle.ViewModels
{
    public class LandingViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IConfigurationService _configurationService;
        private readonly IActivityCategoryService _activityCategoryService;

        public LandingViewModel(
            IDatabaseService databaseService,
            IConfigurationService configurationService,
            IActivityCategoryService activityCategoryService)
        {
            _databaseService = databaseService;
            _configurationService = configurationService;
            _activityCategoryService = activityCategoryService;
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
            await _activityCategoryService.InitializeAsync();
        }

        private static void NavigateToAppShell()
        {
            Application.Current!.MainPage = new AppShell();
        }
    }
}
