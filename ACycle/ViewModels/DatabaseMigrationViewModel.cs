using ACycle.Extensions;
using ACycle.Guards;
using ACycle.Helpers;
using ACycle.Resources.Strings;
using ACycle.Services;
using CommunityToolkit.Mvvm.Input;
using SQLite;

namespace ACycle.ViewModels
{
    public partial class DatabaseMigrationViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IDatabaseMigrationService _databaseMigrationService;

        private string _migrationDatabasePath = string.Empty;
        private string _log = string.Empty;

        public string MigrationDatabasePath
        {
            get => _migrationDatabasePath;
            set
            {
                if (SetProperty(ref _migrationDatabasePath, value))
                {
                    OnPropertyChanged(nameof(MigrationDatabasePathIsNotEmpty));
                    PerformMigrationCommand.NotifyCanExecuteChanged();
                    PerformMigrationAndMergeCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public bool MigrationDatabasePathIsNotEmpty => _migrationDatabasePath != string.Empty;

        public string Log
        {
            get => _log;
            set => SetProperty(ref _log, value);
        }

        public DatabaseMigrationViewModel(
            IDatabaseService databaseService,
            IDatabaseMigrationService databaseMigrationService)
        {
            _databaseService = databaseService;
            _databaseMigrationService = databaseMigrationService;
        }

        [RelayCommand]
        public async Task PickDatabaseFile()
        {
            var fileResult = await FilePicker.Default.PickAsync();
            if (fileResult != null)
            {
                MigrationDatabasePath = fileResult.FullPath;
            }
        }

        [RelayCommand(CanExecute = nameof(MigrationDatabasePathIsNotEmpty))]
        public async Task PerformMigrationAsync()
        {
            Log = "Starting migration...\n";
            await TryToDoAsync(async () =>
            {
                string path = _migrationDatabasePath.Trim().Trim('"');
                FileGuard.Exists(path);
                Log += await _databaseMigrationService.MigrateDatabase(path);
            });
        }

        [RelayCommand(CanExecute = nameof(MigrationDatabasePathIsNotEmpty))]
        public async Task PerformMigrationAndMergeAsync()
        {
            var databasePath = _migrationDatabasePath.Trim().Trim('"');
            SQLiteAsyncConnection? temporaryDatabase = null;

            Log = $"Starting migration and merge...\nCurrent database schema version: {await _databaseService.MainDatabase.GetSchemaAsync()}\n";
            await TryToDoAsync(async () =>
            {
                FileGuard.Exists(databasePath);

                Log += "Making database copy...";
                var temporaryCopyPath = Path.Combine(FileSystem.CacheDirectory, "TempMigrationDatabase.sqlite3");

                await FileHelper.CopyAsync(databasePath, temporaryCopyPath, overWrite: true);
                Log += " done.\n";

                temporaryDatabase = new SQLiteAsyncConnection(temporaryCopyPath);
                Log += "Migration Logs:\n";
                Log += await _databaseMigrationService.MigrateDatabase(temporaryDatabase);

                Log += "Merge Logs:\n";
                Log += await _databaseMigrationService.MergeDatabase(_databaseService.MainDatabase, temporaryDatabase);
            });

            if (temporaryDatabase != null)
                await temporaryDatabase.CloseAsync();
        }

        private async Task TryToDoAsync(Func<Task> job)
        {
            try
            {
                await job();
            }
            catch (Exception ex)
            {
                Log += "\n";
                Log += AppStrings.Text_ExceptionThrownTitle + "\n";
                Log += ex.ToString();
            }
        }
    }
}
