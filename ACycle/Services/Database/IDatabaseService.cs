using SQLite;

namespace ACycle.Services
{
    public interface IDatabaseService : IService
    {
        long SchemaVersion { get; }

        string MainDatabasePath { get; }

        bool IsConnected { get; }

        SQLiteAsyncConnection MainDatabase { get; }

        void ConnectToDatabase(string databasePath);

        void ConnectToDatabase(SQLiteAsyncConnection connection);

        void DisconnectFromDatabase();

        Task DisconnectFromDatabaseAsync();

        Task CreateTablesAsync();
    }
}
