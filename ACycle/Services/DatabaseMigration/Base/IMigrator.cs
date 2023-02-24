using SQLite;

namespace ACycle.Services.DatabaseMigration.Base
{
    public interface IMigrator
    {
        long FromSchemaVersion { get; }

        long ToSchemaVersion { get; }

        Task<string> Migrate(SQLiteAsyncConnection sourceConnection, SQLiteAsyncConnection destinationConnection);
    }
}
