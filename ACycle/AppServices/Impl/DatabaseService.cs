using ACycle.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACycle.AppServices.Impl
{
    public class DatabaseService : IDatabaseService
    {
        public readonly string MainDatabasePath;
        public SQLiteAsyncConnection MainDatabase;

        public DatabaseService()
        {
            MainDatabasePath = Path.Join(FileSystem.AppDataDirectory, "EntryDatabase.sqlite3");
            MainDatabase = new SQLiteAsyncConnection(MainDatabasePath);
        }

        public async Task Start()
        {
            await CreateTables();
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }

        private async Task CreateTables()
        {
            await MainDatabase.CreateTableAsync<EntryEntity>();
        }
    }
}
