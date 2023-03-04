using ACycle.Resources.Strings;
using ACycle.Services;
using CommunityToolkit.Mvvm.Input;

#if WINDOWS
using System.Diagnostics;
#endif

#if ANDROID
using ACycle.Helpers;
#endif

namespace ACycle.ViewModels
{
    public partial class DebuggingViewModel : ViewModelBase
    {
        private readonly IBackupService _backupService;
        private readonly IConfigurationService _configurationService;
        private readonly IDatabaseService _databaseService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IStaticConfigurationService _staticConfigurationService;

        public string NodeUuidTextCellText => $"{AppStrings.Text_Node} UUID";

        public Guid NodeUuid => _configurationService.NodeUuid;

        public string ApplicationNameString => $"{AppInfo.Current.Name} ({AppInfo.Current.PackageName})";

        public string ApplicationVersionString => $"{AppInfo.VersionString} ({AppInfo.BuildString})";

        public string DatabaseSchemaVersionString => $"{_staticConfigurationService.DatabaseSchemaVersion}";

        public DebuggingViewModel(
            IBackupService backupService,
            IConfigurationService configurationService,
            IDatabaseService databaseService,
            IDialogService dialogService,
            INavigationService NavigationService,
            IStaticConfigurationService staticConfigurationService
        )
        {
            _backupService = backupService;
            _configurationService = configurationService;
            _databaseService = databaseService;
            _dialogService = dialogService;
            _navigationService = NavigationService;
            _staticConfigurationService = staticConfigurationService;
        }

#if WINDOWS
        private static async Task OpenDirectoryAsync(string directory)
        {
            var startInfo = new ProcessStartInfo()
            {
                Arguments = directory,
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);

            await Task.Delay(3000);
        }
#endif

        [RelayCommand]
        public static async Task OpenAppDataDirectoryAsync()
        {
#if WINDOWS
            await OpenDirectoryAsync(FileSystem.AppDataDirectory);
#else
            await Task.CompletedTask;
#endif
        }

        [RelayCommand]
        public static async Task OpenCacheDirectoryAsync()
        {
#if WINDOWS
            await OpenDirectoryAsync(FileSystem.CacheDirectory);
#else
            await Task.CompletedTask;
#endif
        }

        [RelayCommand]
        public async Task NavigateToDatabaseMigrationView()
        {
#if WINDOWS || ANDROID
            await _navigationService.NavigateToAsync(AppShell.Route.DatabaseMigrationViewRoute);
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
                var backupFilePath = Path.Combine(
                    Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)!.AbsolutePath,
                    _backupService.GetDatabaseBackupFileName());

                try
                {
                    await _backupService.CreateDatabaseBackup(backupFilePath);
                    await _dialogService.Prompt(AppStrings.Text_DatabaseBackupCompleteTitle, AppStrings.Text_DatabaseBackupCompleteMessage);
                }
                catch (Exception ex)
                {
                    await _dialogService.Prompt(AppStrings.Text_ExceptionThrownTitle, ex.ToString());
                }
            }
            else
            {
                await _dialogService.Prompt(AppStrings.Text_InsufficientApplicationPermission, AppStrings.Text_RequestPermission_WriteStorage);
            }
#endif
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task RestoreDatabaseFromExternalStorage()
        {
#if ANDROID
            var status = await PermissionHelper.CheckAndRequestPermission<Permissions.StorageRead>();
            var backupFile = await FilePicker.Default.PickAsync(new PickOptions { PickerTitle = "ACycle Database" });
            if (backupFile != null)
            {
                var backupFilePath = backupFile.FullPath;
                await _backupService.RestoreDatabaseBackup(backupFilePath);
                await _dialogService.Prompt(AppStrings.Text_DatabaseRestoreCompleteTitle, AppStrings.Text_DatabaseRestoreCompleteMessage);

                await PromptForAppRestart(AppStrings.Text_AppRestartReason_DatabaseRestore);
            }
#endif
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task RestartApplication()
        {
            await PromptForAppRestart(AppStrings.Text_AppRestartReason_UserRequest);
        }

        private async Task PromptForAppRestart(string reason)
        {
            bool shouldRestart = await _dialogService.ConfirmAppRestart(reason);

            if (shouldRestart)
                App.Current()!.Restart();
        }
    }
}
