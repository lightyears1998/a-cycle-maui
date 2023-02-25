using SQLite;

namespace ACycle.Services
{
    public interface IDatabaseMigrationService
    {
        Task<string> MigrateFromDatabase(string migrationDatabasePath);

        Task<string> MigrateFromDatabase(SQLiteAsyncConnection migrationDatabase);
    }
}
