using SQLite;

namespace ACycle.Services
{
    public interface IDatabaseService : IService
    {
        public string MainDatabasePath { get; }

        public SQLiteAsyncConnection MainDatabase { get; }
    }
}
