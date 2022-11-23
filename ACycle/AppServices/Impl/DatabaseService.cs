using ACycle.Entities;
using SQLite;

namespace ACycle.AppServices.Impl
{
    public class SQLiteDatabaseService : IDatabaseService
    {
        public readonly string MainDatabasePath;
        public SQLiteAsyncConnection MainDatabase;

        public SQLiteDatabaseService(string? databasePath = null)
        {
            MainDatabasePath = databasePath ?? Path.Join(FileSystem.AppDataDirectory, "EntryDatabase.sqlite3");
            MainDatabase = new SQLiteAsyncConnection(MainDatabasePath);
        }

        public Task Initialize()
        {
            return CreateTablesAsync();
        }

        private async Task CreateTablesAsync()
        {
            await MainDatabase.CreateTableAsync<EntryEntity>();
            await MainDatabase.CreateTableAsync<EntryHistoryEntity>();
            await MainDatabase.CreateTableAsync<MetadataEntity>();
            await MainDatabase.CreateTableAsync<PeerNodeEntity>();
        }
    }
}
