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
        private readonly IBackupService _backupService;
        private readonly IConfigurationService _configurationService;
        private readonly IDatabaseService _databaseService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IStringLocalizer _stringLocalizer;

        public string NodeUuidTextCellText
        {
            get => $"{_stringLocalizer["Text_Node"]} UUID";
        }

        public Guid NodeUuid => _configurationService.NodeUuid;

        public string ApplicationNameString
        {
            get => $"{AppInfo.Current.Name} ({AppInfo.Current.PackageName})";
        }

        public string ApplicationVersionString
        {
            get => $"{AppInfo.VersionString} ({AppInfo.BuildString})";
        }

        public DebuggingViewModel(
            IBackupService backupService,
            IConfigurationService configurationService,
            IDatabaseService databaseService,
            IDialogService dialogService,
            INavigationService NavigationService,
            IStringLocalizer<AppStrings> stringLocalizer
        )
        {
            _backupService = backupService;
            _configurationService = configurationService;
            _databaseService = databaseService;
            _dialogService = dialogService;
            _navigationService = NavigationService;
            _stringLocalizer = stringLocalizer;
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
                var backupFilePath = Path.Combine(
                    Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)!.AbsolutePath,
                    _backupService.GetDatabseBackupFileName());

                try
                {
                    await _backupService.CreateDatabaseBackup(backupFilePath);
                    await _dialogService.Prompt(_stringLocalizer["Text_DatabaseBackupCompleteTitle"], _stringLocalizer["Text_DatabaseBackupCompleteMessage"]);
                }
                catch (Exception ex)
                {
                    await _dialogService.Prompt(_stringLocalizer["Text_ExcpetionThrownTitle"], ex.ToString());
                }
            }
            else
            {
                await _dialogService.Prompt(_stringLocalizer["Text_InsufficientApplicationPermission"], _stringLocalizer["Text_RequestPermission_WriteStorage"]);
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
                await _dialogService.Prompt(_stringLocalizer["Text_DatabaseRestoreCompleteTitle"], _stringLocalizer["Text_DatabaseRestoreCompleteMessage"]);

                await PromptForAppRestart(_stringLocalizer["Text_AppRestartReason_DatabaseRestore"]);
            }
#endif
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task RestartApplication()
        {
            await PromptForAppRestart(_stringLocalizer["Text_AppRestartReason_UserRequest"]);
        }

        private async Task PromptForAppRestart(string reason)
        {
            bool shouldRestart = await _dialogService.ConfirmAppRestart(reason);

            if (shouldRestart)
                App.Current()!.Restart();
        }
    }
}
