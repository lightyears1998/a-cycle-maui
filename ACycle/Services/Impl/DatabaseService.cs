using ACycle.Entities;
using SQLite;

namespace ACycle.Services.Impl
{
    public class DatabaseService : IDatabaseService
    {
        public string MainDatabasePath { protected set; get; }
        public SQLiteAsyncConnection MainDatabase { protected set; get; }

        public DatabaseService(string? mainDatabasePath = null)
        {
            MainDatabasePath = mainDatabasePath ?? Path.Join(FileSystem.AppDataDirectory, "EntryDatabase.sqlite3");
            MainDatabase = new SQLiteAsyncConnection(MainDatabasePath);
        }

        public async Task InitializeAsync()
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
