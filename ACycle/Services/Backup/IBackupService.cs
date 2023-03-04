namespace ACycle.Services
{
    public interface IBackupService
    {
        Task CreateDatabaseBackupAsync(string backupFilePath);

        string GetDatabaseBackupFileName();

        Task RestoreDatabaseBackupAsync(string backupFilePath);
    }
}
