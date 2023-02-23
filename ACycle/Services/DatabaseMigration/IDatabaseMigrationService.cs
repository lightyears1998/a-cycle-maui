namespace ACycle.Services
{
    public interface IDatabaseMigrationService
    {
        Task<string> MigrateFromDatabaseVersionGodot(string oldDatabasePath);
    }
}
