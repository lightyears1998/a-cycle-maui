using ACycle.Helpers;
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
        private readonly IConfigurationService _configurationService;
        private readonly IDatabaseService _databaseService;
        private readonly IDialogService _dialogService;
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
            IConfigurationService configurationService,
            IDatabaseService databaseService,
            IDialogService dialogService,
            INavigationService NavigationService,
            IStringLocalizer<AppStrings> stringLocalizer
        )
        {
            _configurationService = configurationService;
            _databaseService = databaseService;
            _dialogService = dialogService;
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

        [RelayCommand]
        public async Task BackupDatabaseToExternalStorage()
        {
#if ANDROID
            var status = await PermissionHelper.CheckAndRequestPermission<Permissions.StorageWrite>();
            if (status == PermissionStatus.Granted)
            {
                await _dialogService.Prompt("哇", "好");
            }
            else
            {
                await _dialogService.Prompt("Application Permission", "Please grant this app permission to write external storage.");
            }
#endif
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task RestoreDatabaseFromExternalStorage()
        {
#if ANDROID
#endif
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task RestartApplication()
        {
            bool shouldRestart = await _dialogService.ConfirmAppRestart(_stringLocalizer["Text_AppRestartReason_UserRequest"]);

            if (shouldRestart)
                App.Current()!.Restart();
        }
    }
}
