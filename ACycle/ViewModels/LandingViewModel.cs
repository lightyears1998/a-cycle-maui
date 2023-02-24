using ACycle.Helpers;
using ACycle.Resources.Strings;
using ACycle.Services;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

namespace ACycle.ViewModels
{
    public class LandingViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IConfigurationService _configurationService;
        private readonly IActivityCategoryService _activityCategoryService;
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
            IDatabaseService databaseService,
            IConfigurationService configurationService,
            IActivityCategoryService activityCategoryService,
            IUserService userService,
            IStringLocalizer<AppStrings> stringLocalizer)
        {
            _databaseService = databaseService;
            _configurationService = configurationService;
            _activityCategoryService = activityCategoryService;
            _userService = userService;
            _stringLocalizer = stringLocalizer;
            GetHeadingAndDescription();
        }

        [MemberNotNull(new[] { nameof(_heading), nameof(_description) })]
        private void GetHeadingAndDescription()
        {
            Heading = _stringLocalizer["LandingView_Heading"];
            Description = _stringLocalizer["LandingView_Description"];
        }

        public override async Task InitializeAsync()
        {
            await InitializeBasicServices();
            await SetupAppLanguage();
            GetHeadingAndDescription();

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
