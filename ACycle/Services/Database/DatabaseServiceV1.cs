using ACycle.Entities;
using SQLite;

namespace ACycle.Services
{
    public class DatabaseServiceV1 : DatabaseServiceBase, IDatabaseService
    {
        public override long SchemaVersion => 1;

        public override Type[] Tables => new Type[] {
            typeof(Metadata),
            typeof(ActivityCategoryV1),
            typeof(DiaryV1),
            typeof(NodeHistoryV1),
            typeof(NodePeerV1)
        };

        public DatabaseServiceV1(string mainDatabasePath) : base(mainDatabasePath)
        {
        }

        public DatabaseServiceV1(SQLiteAsyncConnection mainDatabase) : base(mainDatabase)
        {
        }
    }
}
