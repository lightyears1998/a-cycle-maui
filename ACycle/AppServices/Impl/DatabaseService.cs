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

            Task.Run(CreateTables).Wait();
        }

        private async Task CreateTables()
        {
            await MainDatabase.CreateTableAsync<EntryEntity>();
            await MainDatabase.CreateTableAsync<EntryHistoryEntity>();
            await MainDatabase.CreateTableAsync<MetadataEntity>();
            await MainDatabase.CreateTableAsync<PeerNodeEntity>();
        }
    }
}
