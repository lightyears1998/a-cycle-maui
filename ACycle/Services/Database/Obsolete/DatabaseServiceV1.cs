using ACycle.Entities;
using ACycle.Services.Database.Base;
using SQLite;

namespace ACycle.Services
{
    [DatabaseSchema(Version = 1)]
    public class DatabaseServiceV1 : DatabaseServiceBase, IDatabaseService
    {
        public override Type[] Tables => new Type[] {
            typeof(NodeHistoryV1),
            typeof(NodePeerV1)
        }.Concat(EntryBasedEntityTables).ToArray();

        public override Type[] EntryBasedEntityTables => new Type[]
        {
            typeof(ActivityCategoryV1),
            typeof(DiaryV1),
        };

        public DatabaseServiceV1(IStaticConfigurationService staticConfiguration) : base(staticConfiguration)
        {
        }

        public DatabaseServiceV1(string mainDatabasePath) : base(mainDatabasePath)
        {
        }

        public DatabaseServiceV1(SQLiteAsyncConnection mainDatabase) : base(mainDatabase)
        {
        }
    }
}
