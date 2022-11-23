using SQLite;

namespace ACycle.AppServices
{
    public interface IDatabaseService : IAppService
    {
        public string MainDatabasePath { get; }

        public SQLiteAsyncConnection MainDatabase { get; }
    }
}
