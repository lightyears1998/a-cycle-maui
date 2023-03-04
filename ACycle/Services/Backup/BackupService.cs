using ACycle.Helpers;

namespace ACycle.Services
{
    public class BackupService : IBackupService
    {
        private readonly IDatabaseService _databaseService;

        public BackupService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public string GetDatabaseBackupFileName()
        {
            return $"MainDatabase_{DateTime.Now:yyyy-MM-ddTHHmmss}.sqlite3";
        }

        public async Task CreateDatabaseBackupAsync(string backupFilePath)
        {
            await FileHelper.CopyAsync(_databaseService.MainDatabasePath, backupFilePath);
        }

        public async Task RestoreDatabaseBackupAsync(string backupFilePath)
        {
            await _databaseService.DisconnectFromDatabaseAsync();
            await FileHelper.CopyAsync(backupFilePath, _databaseService.MainDatabasePath, overWrite: true);
        }
    }
}
