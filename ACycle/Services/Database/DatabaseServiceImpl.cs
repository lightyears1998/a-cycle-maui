using SQLite;

namespace ACycle.Services
{
    public class DatabaseServiceImpl : DatabaseServiceV2
    {
        public DatabaseServiceImpl(IStaticConfigurationService staticConfiguration) : base(staticConfiguration)
        {
        }

        public DatabaseServiceImpl(string mainDatabasePath) : base(mainDatabasePath)
        {
        }

        public DatabaseServiceImpl(SQLiteAsyncConnection mainDatabase) : base(mainDatabase)
        {
        }
    }
}
