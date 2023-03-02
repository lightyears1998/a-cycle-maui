using ACycle.Entities;
using ACycle.Extensions;
using ACycle.Repositories;
using ACycle.Services.Database;
using ACycle.Services.Database.Base;
using CommunityToolkit.Diagnostics;
using SQLite;

namespace ACycle.Services
{
    public abstract class DatabaseServiceBase : Service, IDatabaseService
    {
        public long SchemaVersion { get; private set; }

        public virtual Type[] Tables => new Type[] {
            typeof(Metadata)
        };

        public abstract Type[] EntryBasedEntityTables { get; }

        private SQLiteAsyncConnection? _mainDatabase = null;

        public string MainDatabasePath => MainDatabase.DatabasePath;

        public SQLiteAsyncConnection MainDatabase
        {
            get => _mainDatabase!;
        }

        public bool IsConnected => _mainDatabase != null;

        public DatabaseServiceBase(IStaticConfigurationService staticConfiguration) : this(staticConfiguration.MainDatabasePath)
        {
        }

        public DatabaseServiceBase(string mainDatabasePath)
        {
            SchemaVersion = GetSchemaVersionOfDatabaseService(GetType());
            ConnectToDatabase(mainDatabasePath);
        }

        public static long GetSchemaVersionOfDatabaseService(Type type)
        {
            var schemaAttribute = (DatabaseSchemaAttribute)Attribute.GetCustomAttribute(type, typeof(DatabaseSchemaAttribute))!;
            Guard.IsNotNull(schemaAttribute);

            return schemaAttribute.Version;
        }

        public DatabaseServiceBase(SQLiteAsyncConnection mainDatabase)
        {
            ConnectToDatabase(mainDatabase);
        }

        public void ConnectToDatabase(string mainDatabasePath)
        {
            Guard.IsNotNullOrWhiteSpace(mainDatabasePath);

            DisconnectFromDatabase();
            _mainDatabase = new SQLiteAsyncConnection(mainDatabasePath);
        }

        public void ConnectToDatabase(SQLiteAsyncConnection connection)
        {
            DisconnectFromDatabase();
            _mainDatabase = connection;
        }

        public void DisconnectFromDatabase()
        {
            if (_mainDatabase != null)
            {
                _ = _mainDatabase.CloseAsync();
                _mainDatabase = null;
            }
        }

        public async Task DisconnectFromDatabaseAsync()
        {
            if (_mainDatabase != null)
            {
                await _mainDatabase.CloseAsync();
                _mainDatabase = null;
            }
        }

        public override async Task InitializeAsync()
        {
            await CreateTablesAsync();
            await CreateSchemaIfNotPresent();
        }

        public virtual async Task CreateTablesAsync()
        {
            foreach (var table in Tables)
            {
                await MainDatabase.CreateTableAsync(table);
            }
        }

        public async Task CreateSchemaIfNotPresent()
        {
            long? schema = await MainDatabase.GetSchemaAsync();
            if (schema == null)
            {
                await MainDatabase.SetSchemaAsync(SchemaVersion);
            }
        }

        public async Task<int> CountEntries()
        {
            int count = 0;
            foreach (var entryType in EntryBasedEntityTables)
            {
                var getTableMethod = typeof(SQLiteAsyncConnection).GetMethod(nameof(SQLiteAsyncConnection.Table));
                Guard.IsNotNull(getTableMethod);

                var dedicateGetTableMethod = getTableMethod.MakeGenericMethod(entryType);
                Guard.IsNotNull(dedicateGetTableMethod);

                dynamic? table = dedicateGetTableMethod.Invoke(_mainDatabase, null);
                Guard.IsNotNull(table);

                count += await table!.CountAsync();
            }
            return count;
        }

        public virtual async Task MergeDatabase(SQLiteAsyncConnection mergingDatabase)
        {
            await MergeEntryBasedEntityTables(mergingDatabase);
        }

        private async Task MergeEntryBasedEntityTables(SQLiteAsyncConnection mergingDatabase)
        {
            foreach (var entryType in EntryBasedEntityTables)
            {
                var dedicateRepositoryType = typeof(EntryRepository<>).MakeGenericType(entryType);
                dynamic baseRepository = Activator.CreateInstance(dedicateRepositoryType, new IDatabaseConnectionWrapper?[] { this })!;
                dynamic mergingRepository = Activator.CreateInstance(dedicateRepositoryType, new IDatabaseConnectionWrapper?[] { new DatabaseConnectionWrapper(mergingDatabase) })!;
                Guard.IsNotNull(baseRepository);
                Guard.IsNotNull(mergingRepository);

                dynamic allMergingEntries = await mergingRepository!.FindAllAsync();
                Guard.IsNotNull(allMergingEntries);

                await baseRepository!.SaveIfFresherAsync(allMergingEntries);
            }
        }
    }
}
