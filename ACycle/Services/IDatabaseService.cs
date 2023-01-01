using SQLite;

namespace ACycle.Services
{
    public interface IDatabaseService : IAppService
    {
        public string MainDatabasePath { get; }

        public SQLiteAsyncConnection MainDatabase { get; }
    }
}
