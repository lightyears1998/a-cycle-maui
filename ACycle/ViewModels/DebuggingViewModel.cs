﻿using ACycle.Helpers;
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
                var mainDbPath = _databaseService.MainDatabasePath;
                var backupDbPath = Path.Combine(
                    Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)!.AbsolutePath,
                    $"MainDatabase_{DateTime.Now:yyyy-MM-ddTHHmmss}.sqlite3");

                try
                {
                    await FileHelper.CopyAsync(mainDbPath, backupDbPath);
                    await _dialogService.Prompt("Backup", "Backup completed.");
                }
                catch (Exception ex)
                {
                    await _dialogService.Prompt("Oh...", ex.ToString());
                }
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
            var status = await PermissionHelper.CheckAndRequestPermission<Permissions.StorageRead>();
            var backupFile = await FilePicker.Default.PickAsync(new PickOptions { PickerTitle = "ACycle Database" });
            if (backupFile != null)
            {
                var backupFilePath = backupFile.FullPath;
                await _databaseService.DisconnectFromDatabaseAsync();
                await FileHelper.CopyAsync(backupFilePath, _databaseService.MainDatabasePath, overWrite: true);
                await _dialogService.Prompt("Database Restore", "Database is restored successfully.");
            }
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
