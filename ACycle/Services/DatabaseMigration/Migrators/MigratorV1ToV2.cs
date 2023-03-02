using SQLite;

namespace ACycle.Services.DatabaseMigration.Migrators
{
    [MigratorSchemaVersion(Source = 1, Destination = 2)]
    public class MigratorV1ToV2 : Migrator
    {
        public override async Task<string> MigrateAsync(SQLiteAsyncConnection connection)
        {
            var statements = new string[]
            {
                "ALTER TABLE node_history RENAME COLUMN model_uuid TO 'entry_uuid'",
                "ALTER TABLE node_history RENAME COLUMN model_updated_at TO 'entry_updated_at'",
                "ALTER TABLE node_history RENAME COLUMN model_updated_by TO 'entry_updated_by'"
            };

            foreach (var sql in statements)
            {
                await connection.ExecuteAsync(sql);
            }

            return "Migrate table `node_history` successfully.";
        }
    }
}
