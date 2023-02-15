using ACycle.Entities;
using ACycle.Repositories;
using SQLite;

namespace ACycle.Services
{
    public class DatabaseService : Service, IDatabaseService
    {
        private readonly MetadataService _metadataService;

        public readonly long CURRENT_SCHEMA_VERSION = 1;

        public string DatabaseDirectory { protected set; get; }

        public string MainDatabasePath { protected set; get; }

        public SQLiteAsyncConnection MainDatabase { protected set; get; }

        public DatabaseService(string databaseDirectory)
        {
            DatabaseDirectory = databaseDirectory;
            MainDatabasePath = Path.Join(DatabaseDirectory, "MainDatabase.sqlite3");

            MainDatabase = new SQLiteAsyncConnection(MainDatabasePath);
            _metadataService = new MetadataService(new MetadataRepository(this));
        }

        public override async Task InitializeAsync()
        {
            await MainDatabase.CreateTableAsync<Metadata>();
            long schemaVersion = await ReadSchemaVersionAsync();
            if (schemaVersion != CURRENT_SCHEMA_VERSION)
            {
                await BumpSchemaAsync();
            }
            await CreateTablesAsync();
        }

        private async Task<long> ReadSchemaVersionAsync()
        {
            return Convert.ToInt64(await _metadataService.GetMetadataAsync("SCHEMA", "1"));
        }

        private Task BumpSchemaAsync()
        {
            throw new NotImplementedException();
        }

        private async Task CreateTablesAsync()
        {
            await MainDatabase.CreateTableAsync<ActivityCategoryV1>();
            await MainDatabase.CreateTableAsync<DiaryV1>();
            await MainDatabase.CreateTableAsync<NodeHistoryV1>();
            await MainDatabase.CreateTableAsync<NodePeerV1>();
        }
    }
}
