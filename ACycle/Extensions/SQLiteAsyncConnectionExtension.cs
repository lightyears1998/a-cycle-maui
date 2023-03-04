using SQLite;

namespace ACycle.Extensions
{
    public static class SQLiteAsyncConnectionExtension
    {
        /// <summary>
        /// Get the schema version of the connection.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>The schema version number; `null` if no schema version is found.</returns>
        public static async Task<long?> GetSchemaAsync(this SQLiteAsyncConnection connection)
        {
            long? schemaVersion = null;

            try
            {
                var result = await connection.QueryScalarsAsync<long>("SELECT value FROM metadata WHERE key = 'SCHEMA'");
                if (result.Count > 0)
                {
                    schemaVersion = result[0];
                }
            }
            catch (SQLiteException) { }

            return schemaVersion;
        }

        public static async Task SetSchemaAsync(this SQLiteAsyncConnection connection, long schemaVersion)
        {
            await connection.ExecuteAsync("INSERT INTO metadata(key, value, updated_at) VALUES('SCHEMA', ?, ?)" +
                " ON CONFLICT(key) DO UPDATE SET value=?, updated_at=?",
                new object[] { schemaVersion, DateTime.Now.Ticks, schemaVersion, DateTime.Now.Ticks });
        }
    }
}
