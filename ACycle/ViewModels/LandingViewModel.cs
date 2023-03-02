using ACycle.Helpers;
using ACycle.Resources.Strings;
using ACycle.Services;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace ACycle.ViewModels
{
    public class LandingViewModel : ViewModelBase
    {
        private readonly ILogger _logger;
        private readonly IActivityCategoryService _activityCategoryService;
        private readonly IConfigurationService _configurationService;
        private readonly IDatabaseService _databaseService;
        private readonly IDatabaseMigrationService _databaseMigrationService;
        private readonly IUserService _userService;

        private string _heading;
        private string _description;

        public string Heading
        {
            get => _heading;

            [MemberNotNull(nameof(_heading))]
            set => SetProperty(ref _heading, value);
        }

        public string Description
        {
            get => _description;

            [MemberNotNull(nameof(_description))]
            set => SetProperty(ref _description, value);
        }

        public LandingViewModel(
            ILogger<LandingViewModel> logger,
            IActivityCategoryService activityCategoryService,
            IConfigurationService configurationService,
            IDatabaseService databaseService,
            IDatabaseMigrationService databaseMigrationService,
            IUserService userService)
        {
            _logger = logger;
            _activityCategoryService = activityCategoryService;
            _configurationService = configurationService;
            _databaseService = databaseService;
            _databaseMigrationService = databaseMigrationService;
            _userService = userService;
            GetHeadingAndDescriptionText();
        }

        [MemberNotNull(new[] { nameof(_heading), nameof(_description) })]
        private void GetHeadingAndDescriptionText()
        {
            Heading = AppStrings.LandingView_Heading;
            Description = AppStrings.LandingView_Description;
        }

        public override async Task InitializeAsync()
        {
            await InitializeBasicServices();

            await SetupAppLanguage();
            GetHeadingAndDescriptionText();

            await PerformDatabaseMigration();

            await InitializeAdvancedServices();

            NavigateToAppShell();
        }

        private async Task InitializeBasicServices()
        {
            await _databaseService.InitializeAsync();
            await _configurationService.InitializeAsync();

            _logger.LogDebug("Basic services are initialized.");
        }

        private async Task SetupAppLanguage()
        {
            var userInfo = await _userService.GetUserInfoAsync();
            if (userInfo.PreferredLanguage != null)
            {
                LanguageHelper.SwitchLanguage(userInfo.PreferredLanguage);
            }

            _logger.LogDebug("Language settings are initialized.");
        }

        private async Task PerformDatabaseMigration()
        {
            await _databaseMigrationService.MigrateDatabase(_databaseService.MainDatabase);

            _logger.LogDebug("Database migration is finished.");
        }

        private async Task InitializeAdvancedServices()
        {
            await _activityCategoryService.InitializeAsync();

            _logger.LogDebug("Advanced services are initialized.");
        }

        private void NavigateToAppShell()
        {
            Application.Current!.MainPage = new AppShell();

            _logger.LogDebug("Navigated to AppShell.");
        }
    }
}
