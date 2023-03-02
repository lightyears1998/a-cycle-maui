using ACycle.Entities;
using ACycle.Services.Database.Base;
using SQLite;

namespace ACycle.Services
{
    [DatabaseSchema(Version = 2)]
    public class DatabaseServiceV2 : DatabaseServiceBase, IDatabaseService
    {
        public override Type[] Tables => new Type[] {
            typeof(NodeHistoryV2),
            typeof(NodePeerV1)
        }.Concat(EntryBasedEntityTables).Concat(base.Tables).ToArray();

        public override Type[] EntryBasedEntityTables => new Type[]
        {
            typeof(ActivityCategoryV1),
            typeof(DiaryV1),
        };

        public DatabaseServiceV2(IStaticConfigurationService staticConfiguration) : base(staticConfiguration)
        {
        }

        public DatabaseServiceV2(string mainDatabasePath) : base(mainDatabasePath)
        {
        }

        public DatabaseServiceV2(SQLiteAsyncConnection mainDatabase) : base(mainDatabase)
        {
        }
    }
}
