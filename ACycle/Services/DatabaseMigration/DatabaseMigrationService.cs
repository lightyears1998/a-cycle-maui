using ACycle.Extensions;
using ACycle.Services.DatabaseMigration;
using SQLite;
using System.Text;

namespace ACycle.Services
{
    public class DatabaseMigrationService : IDatabaseMigrationService
    {
        private readonly IDictionary<long, Lazy<IMigrator>> _schemaMigratorMap = new Dictionary<long, Lazy<IMigrator>>();

        public Task<string> MigrateDatabase(string migrationDatabasePath)
        {
            return MigrateDatabase(new SQLiteAsyncConnection(migrationDatabasePath));
        }

        public async Task<string> MigrateDatabase(SQLiteAsyncConnection migrationDatabase)
        {
            StringBuilder migrationResultBuilder = new();
            long? lastSchemaVersion = null;
            long? schemaVersion = await migrationDatabase.GetSchemaAsync();

            if (!schemaVersion.HasValue)
            {
                var migrationResult = await MigrateFromUnknownSchema(migrationDatabase);
                migrationResultBuilder.AppendLine(migrationResult);
                schemaVersion = await migrationDatabase.GetSchemaAsync();
            }

            while (schemaVersion.HasValue && schemaVersion != lastSchemaVersion)
            {
                lastSchemaVersion = schemaVersion;

                Lazy<IMigrator>? migrator;
                _schemaMigratorMap.TryGetValue(schemaVersion.Value, out migrator);

                if (migrator == null)
                {
                    break;
                }
                var migrationResult = await migrator.Value.MigrateAsync(migrationDatabase, migrationDatabase);
                migrationResultBuilder.AppendLine(migrationResult);

                schemaVersion = await migrationDatabase.GetSchemaAsync();
            }

            return migrationResultBuilder.ToString();
        }

        public async Task<string> MigrateFromUnknownSchema(SQLiteAsyncConnection migrationDatabase)
        {
            var migratorV0 = new MigratorV0ToV1();
            var isGodotVersionDatabase = await MigratorV0ToV1.CheckIfGodotVersionDatabaseAsync(migrationDatabase);

            if (!isGodotVersionDatabase)
            {
                return "Source database is not a valid database, because its schema is unknown, and it is not a ACycle Godot version database.";
            }

            return await migratorV0.MigrateAsync(migrationDatabase, migrationDatabase);
        }

        /// <summary>
        /// Merge the merging database into the base database.
        /// </summary>
        /// <param name="baseDatabase">The base database</param>
        /// <param name="mergingDatabase">The database that is being merged into the base database</param>
        public async Task<string> MergeDatabase(SQLiteAsyncConnection baseDatabase, SQLiteAsyncConnection mergingDatabase)
        {
            StringBuilder mergeResultBuilder = new();

            var databaseService = new DatabaseServiceV1(baseDatabase);

            mergeResultBuilder.AppendLine($"Entry count before merging: {await databaseService.CountEntries()}");
            await databaseService.MergeDatabase(mergingDatabase);
            mergeResultBuilder.AppendLine($"Entry count after merging: {await databaseService.CountEntries()}");

            return mergeResultBuilder.ToString();
        }
    }
}
