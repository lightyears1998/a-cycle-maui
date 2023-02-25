using SQLite;

namespace ACycle.Services.Database.Obsolete
{
    public class DatabaseServiceV0 : IDatabaseService
    {
        public long SchemaVersion => 0;

        public string MainDatabasePath => throw new NotImplementedException();

        public bool IsConnected => throw new NotImplementedException();

        public SQLiteAsyncConnection MainDatabase => throw new NotImplementedException();

        public void ConnectToDatabase(string databasePath)
        {
            throw new NotImplementedException();
        }

        public void ConnectToDatabase(SQLiteAsyncConnection connection)
        {
            throw new NotImplementedException();
        }

        public void DisconnectFromDatabase()
        {
            throw new NotImplementedException();
        }

        public Task DisconnectFromDatabaseAsync()
        {
            throw new NotImplementedException();
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public Task CreateTablesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
