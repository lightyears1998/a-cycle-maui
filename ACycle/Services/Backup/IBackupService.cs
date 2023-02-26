namespace ACycle.Services
{
    public interface IBackupService
    {
        Task CreateDatabaseBackup(string backupFilePath);

        string GetDatabaseBackupFileName();

        Task RestoreDatabaseBackup(string backupFilePath);
    }
}
