using SQLite;

namespace ACycle.Services
{
    public interface IDatabaseService : IService
    {
        string MainDatabasePath { get; }

        SQLiteAsyncConnection MainDatabase { get; }

        void ConnectToDatabase();

        Task DisconnectFromDatabaseAsync();
    }
}
