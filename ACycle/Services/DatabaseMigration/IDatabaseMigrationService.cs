namespace ACycle.Services
{
    public interface IDatabaseMigrationService
    {
        Task<string> MigrateFromDatabase(string migrationDatabasePath);
    }
}
