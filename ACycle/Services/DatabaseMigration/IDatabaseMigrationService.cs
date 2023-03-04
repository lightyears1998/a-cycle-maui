using SQLite;

namespace ACycle.Services
{
    public interface IDatabaseMigrationService
    {
        Task<string> MergeDatabaseAsync(SQLiteAsyncConnection baseDatabase, SQLiteAsyncConnection mergingDatabase);

        Task<string> MigrateDatabaseAsync(string migrationDatabasePath);

        Task<string> MigrateDatabaseAsync(SQLiteAsyncConnection migrationDatabase);
    }
}
