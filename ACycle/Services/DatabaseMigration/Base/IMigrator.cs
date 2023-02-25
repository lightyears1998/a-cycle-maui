using SQLite;

namespace ACycle.Services.DatabaseMigration
{
    public interface IMigrator
    {
        long SourceSchemaVersion { get; }

        long DestinationSchemaVersion { get; }

        Task<string> MigrateAsync(SQLiteAsyncConnection sourceConnection, SQLiteAsyncConnection destinationConnection);
    }
}
