using SQLite;

namespace ACycle.Services.Database
{
    public interface IDatabaseConnectionWrapper
    {
        string MainDatabasePath { get; }

        SQLiteAsyncConnection MainDatabase { get; }
    }
}
