using ACycle.Services;
using CommunityToolkit.Mvvm.Input;

namespace ACycle.ViewModels
{
    public partial class DatabaseMigrationViewModel : ViewModelBase
    {
        private IDatabaseMigrationService _databaseMigrationService;

        private string _migrationDatabasePath = string.Empty;
        private string _migrationPreview = string.Empty;

        public string MigrationDatabasePath
        {
            get => _migrationDatabasePath;
            set
            {
                if (SetProperty(ref _migrationDatabasePath, value))
                {
                    OnPropertyChanged(nameof(MigrationDatabasePathIsNotEmpty));
                }
            }
        }

        public bool MigrationDatabasePathIsNotEmpty => _migrationDatabasePath != string.Empty;

        public string MigrationLog
        {
            get => _migrationPreview;
            set => SetProperty(ref _migrationPreview, value);
        }

        public DatabaseMigrationViewModel(IDatabaseMigrationService databaseMigrationService)
        {
            _databaseMigrationService = databaseMigrationService;
        }

        [RelayCommand(CanExecute = nameof(MigrationDatabasePathIsNotEmpty))]
        public async Task PerformMigration()
        {
            MigrationLog = await _databaseMigrationService.MigrateFromDatabase(_migrationDatabasePath.Trim().Trim('"'));
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
    }
}
