using ACycle.Entities;
using SQLite;
using System.Dynamic;

namespace ACycle.AppServices.Impl
{
    public class DatabaseService : IDatabaseService
    {
        public readonly string MainDatabasePath;
        public SQLiteAsyncConnection MainDatabase;

        public DatabaseService(string? databasePath = null)
        {
            MainDatabasePath = databasePath ?? Path.Join(FileSystem.AppDataDirectory, "EntryDatabase.sqlite3");
            MainDatabase = new SQLiteAsyncConnection(MainDatabasePath);
        }

        public async Task Initialize()
        {
            await CreateTablesAsync();
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
