using ACycle.Entities;
using SQLite;

namespace ACycle.Services
{
    public class DatabaseService : Service, IDatabaseService
    {
        public string MainDatabasePath { protected set; get; }
        public SQLiteAsyncConnection MainDatabase { protected set; get; }

        public DatabaseService(string? mainDatabasePath = null)
        {
            MainDatabasePath = mainDatabasePath ?? Path.Join(FileSystem.AppDataDirectory, "MainDatabase.sqlite3");
            MainDatabase = new SQLiteAsyncConnection(MainDatabasePath);
        }

        public override async Task InitializeAsync()
        {
            await CreateTablesAsync();
        }

        private async Task CreateTablesAsync()
        {
            await MainDatabase.CreateTableAsync<Entities.Entry>();
            await MainDatabase.CreateTableAsync<EntryHistory>();
            await MainDatabase.CreateTableAsync<Metadata>();
            await MainDatabase.CreateTableAsync<PeerNode>();
        }
    }
}
