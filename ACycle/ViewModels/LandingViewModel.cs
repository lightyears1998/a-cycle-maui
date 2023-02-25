using ACycle.Helpers;
using ACycle.Resources.Strings;
using ACycle.Services;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

namespace ACycle.ViewModels
{
    public class LandingViewModel : ViewModelBase
    {
        private readonly IActivityCategoryService _activityCategoryService;
        private readonly IConfigurationService _configurationService;
        private readonly IDatabaseService _databaseService;
        private readonly IDatabaseMigrationService _databaseMigrationService;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<AppStrings> _stringLocalizer;

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
            IActivityCategoryService activityCategoryService,
            IConfigurationService configurationService,
            IDatabaseService databaseService,
            IDatabaseMigrationService databaseMigrationService,
            IUserService userService,
            IStringLocalizer<AppStrings> stringLocalizer)
        {
            _activityCategoryService = activityCategoryService;
            _configurationService = configurationService;
            _databaseService = databaseService;
            _databaseMigrationService = databaseMigrationService;
            _userService = userService;
            _stringLocalizer = stringLocalizer;
            GetHeadingAndDescriptionText();
        }

        [MemberNotNull(new[] { nameof(_heading), nameof(_description) })]
        private void GetHeadingAndDescriptionText()
        {
            Heading = _stringLocalizer["LandingView_Heading"];
            Description = _stringLocalizer["LandingView_Description"];
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
        }

        private async Task SetupAppLanguage()
        {
            var userInfo = await _userService.GetUserInfoAsync();
            if (userInfo.PreferredLanguage != null)
            {
                LanguageHelper.SwitchLanguage(userInfo.PreferredLanguage);
            }
        }

        private async Task PerformDatabaseMigration()
        {
            await _databaseMigrationService.MigrateFromDatabase(_databaseService.MainDatabase);
        }

        private async Task InitializeAdvancedServices()
        {
            await _activityCategoryService.InitializeAsync();
        }

        private static void NavigateToAppShell()
        {
            Application.Current!.MainPage = new AppShell();
        }
    }
}
