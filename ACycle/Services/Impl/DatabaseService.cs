using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACycle.Services.Impl
{
    public class DatabaseService : IDatabaseService
    {
        public readonly string EntryDatabasePath;

        public DatabaseService()
        {
            EntryDatabasePath = Path.Join(FileSystem.AppDataDirectory, "EntryDatabase.sqlite3");
        }
    }
}
