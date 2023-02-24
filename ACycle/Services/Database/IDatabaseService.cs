using SQLite;

namespace ACycle.Services
{
    public interface IDatabaseService : IService
    {
        long SchemaVersion { get; }

        string MainDatabasePath { get; }

        SQLiteAsyncConnection MainDatabase { get; }

        void ConnectToDatabase();

        Task DisconnectFromDatabaseAsync();
    }
}
