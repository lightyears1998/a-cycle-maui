using ACycle.Services.DatabaseMigration.Base;
using SQLite;

namespace ACycle.Services.DatabaseMigration
{
    public class MigratorV0ToV1 : IMigrator
    {
        public long FromSchemaVersion => 0;

        public long ToSchemaVersion => 1;

        public Task<string> Migrate(SQLiteAsyncConnection sourceConnection, SQLiteAsyncConnection destinationConnection)
        {
            throw new NotImplementedException();
        }
    }
}
