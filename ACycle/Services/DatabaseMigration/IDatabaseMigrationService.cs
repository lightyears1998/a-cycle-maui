using SQLite;

namespace ACycle.Services
{
    public interface IDatabaseMigrationService
    {
        Task<string> MergeDatabase(SQLiteAsyncConnection baseDatabase, SQLiteAsyncConnection mergingDatabase);

        Task<string> MigrateDatabase(string migrationDatabasePath);

        Task<string> MigrateDatabase(SQLiteAsyncConnection migrationDatabase);
    }
}
