using ACycle.Resources.Strings;
using ACycle.Services;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Localization;

#if WINDOWS
using System.Diagnostics;
#endif

namespace ACycle.ViewModels
{
    public partial class DebuggingViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IConfigurationService _configurationService;
        private readonly INavigationService _navigationService;
        private readonly IStringLocalizer _stringLocalizer;

        public string NodeUuidLabelText
        {
            get => $"{_stringLocalizer["Text_Node"]} UUID: {_configurationService.NodeUuid}";
        }

        public string ApplicationNameString
        {
            get => $"{AppInfo.Current.Name} ({AppInfo.Current.PackageName})";
        }

        public string ApplicationVersionString
        {
            get => $"{AppInfo.VersionString} ({AppInfo.BuildString})";
        }

        public DebuggingViewModel(
            IDatabaseService databaseService,
            IConfigurationService configurationService,
            INavigationService NavigationService,
            IStringLocalizer<AppStrings> stringLocalizer
        )
        {
            _databaseService = databaseService;
            _configurationService = configurationService;
            _navigationService = NavigationService;
            _stringLocalizer = stringLocalizer;
        }

        [RelayCommand]
        public static async Task OpenDataDirectoryAsync()
        {
#if WINDOWS
            var startInfo = new ProcessStartInfo()
            {
                Arguments = FileSystem.AppDataDirectory,
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);

            await Task.Delay(3000);
#else
            await Task.CompletedTask;
#endif
        }

        [RelayCommand]
        public async Task NavigateToDatabaseMigrationView()
        {
#if WINDOWS
            await _navigationService.NavigateToAsync("DatabaseMigration");
#endif
            await Task.CompletedTask;
        }
    }
}
