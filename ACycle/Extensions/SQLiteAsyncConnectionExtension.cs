using SQLite;

namespace ACycle.Extensions
{
    public static class SQLiteAsyncConnectionExtension
    {
        /// <summary>
        /// Get schema version of the connection.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>The schema version number; 0 if no schema version is found.</returns>
        public static async Task<long?> GetSchemaAsync(this SQLiteAsyncConnection connection)
        {
            long? schemaVersion = null;

            try
            {
                var result = await connection.QueryScalarsAsync<long>("SELECT value FROM metadata WHERE key = 'SCHEMA'");
                if (result.Count() > 0)
                {
                    schemaVersion = result[0];
                }
            }
            catch (SQLiteException) { }

            return schemaVersion;
        }
    }
}
