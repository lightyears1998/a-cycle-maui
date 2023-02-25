using CommunityToolkit.Diagnostics;
using SQLite;

namespace ACycle.Services
{
    public abstract class DatabaseServiceBase : Service, IDatabaseService
    {
        public abstract long SchemaVersion { get; }

        public abstract Type[] Tables { get; }

        private SQLiteAsyncConnection? _mainDatabase = null;

        public string MainDatabasePath => MainDatabase.DatabasePath;

        public SQLiteAsyncConnection MainDatabase
        {
            get => _mainDatabase!;
        }

        public bool IsConnected => _mainDatabase != null;

        public DatabaseServiceBase(string mainDatabasePath)
        {
            ConnectToDatabase(mainDatabasePath);
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
        }

        public virtual async Task CreateTablesAsync()
        {
            foreach (var table in Tables)
            {
                await MainDatabase.CreateTableAsync(table);
            }
        }
    }
}
