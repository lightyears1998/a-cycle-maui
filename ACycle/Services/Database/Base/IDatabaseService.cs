using ACycle.Services.Database;
using SQLite;

namespace ACycle.Services
{
    public interface IDatabaseService : IService, IDatabaseConnectionWrapper
    {
        long SchemaVersion { get; }

        Type[] Tables { get; }

        Type[] EntryBasedEntityTables { get; }

        bool IsConnected { get; }

        void ConnectToDatabase(string databasePath);

        void ConnectToDatabase(SQLiteAsyncConnection connection);

        void DisconnectFromDatabase();

        Task DisconnectFromDatabaseAsync();

        Task CreateTablesAsync();

        Task MergeDatabaseAsync(SQLiteAsyncConnection mergingDatabase);

        Task<int> CountEntriesAsync();
    }
}
