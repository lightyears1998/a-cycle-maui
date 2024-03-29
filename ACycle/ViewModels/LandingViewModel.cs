﻿using ACycle.Helpers;
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
        private readonly ISynchronizationService _syncService;
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
            IUserService userService,
            ISynchronizationService syncService)
        {
            _logger = logger;
            _activityCategoryService = activityCategoryService;
            _configurationService = configurationService;
            _databaseService = databaseService;
            _databaseMigrationService = databaseMigrationService;
            _userService = userService;
            _syncService = syncService;

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
            await InitializeBasicServicesAsync().ConfigureAwait(false);

            await SetupAppLanguageAsync().ConfigureAwait(false);
            GetHeadingAndDescriptionText();

            await PerformDatabaseMigrationAsync().ConfigureAwait(false);

            await InitializeAdvancedServicesAsync().ConfigureAwait(false);

            NavigateToAppShell();
        }

        private async Task InitializeBasicServicesAsync()
        {
            await _databaseService.InitializeAsync().ConfigureAwait(false);
            await _configurationService.InitializeAsync().ConfigureAwait(false);

            _logger.LogDebug("Basic services are initialized.");
        }

        private async Task SetupAppLanguageAsync()
        {
            var userInfo = await _userService.GetUserInfoAsync().ConfigureAwait(false);
            if (userInfo.PreferredLanguage != null)
            {
                LanguageHelper.SwitchLanguage(userInfo.PreferredLanguage);
            }

            _logger.LogDebug("Language settings are initialized.");
        }

        private async Task PerformDatabaseMigrationAsync()
        {
            await _databaseMigrationService.MigrateDatabaseAsync(_databaseService.MainDatabase).ConfigureAwait(false);

            _logger.LogDebug("Database migration is finished.");
        }

        private async Task InitializeAdvancedServicesAsync()
        {
            await _databaseService.CreateTablesAsync().ConfigureAwait(false);
            await _activityCategoryService.InitializeAsync().ConfigureAwait(false);
            await _syncService.InitializeAsync().ConfigureAwait(false);

            _logger.LogDebug("Advanced services are initialized.");
        }

        private void NavigateToAppShell()
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Application.Current!.MainPage = new AppShell();

                _logger.LogDebug("Navigated to AppShell.");
            });
        }
    }
}
