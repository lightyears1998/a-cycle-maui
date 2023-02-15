using ACycle.Helpers;
using ACycle.Services;

namespace ACycle.ViewModels
{
    public class LandingViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IConfigurationService _configurationService;
        private readonly IActivityCategoryService _activityCategoryService;
        private readonly IUserService _userService;

        public LandingViewModel(
            IDatabaseService databaseService,
            IConfigurationService configurationService,
            IActivityCategoryService activityCategoryService,
            IUserService userService)
        {
            _databaseService = databaseService;
            _configurationService = configurationService;
            _activityCategoryService = activityCategoryService;
            _userService = userService;
        }

        public override async Task InitializeAsync()
        {
            await InitializeServices();
            await SetupAppLanguage();
            NavigateToAppShell();
        }

        private async Task InitializeServices()
        {
            await _databaseService.InitializeAsync();
            await _configurationService.InitializeAsync();
            await _activityCategoryService.InitializeAsync();
        }

        private async Task SetupAppLanguage()
        {
            var userInfo = await _userService.GetUserInfoAsync();
            if (userInfo.PreferredLanguage != null)
            {
                LanguageHelper.SwitchLanguage(userInfo.PreferredLanguage);
            }
        }

        private static void NavigateToAppShell()
        {
            Application.Current!.MainPage = new AppShell();
        }
    }
}
