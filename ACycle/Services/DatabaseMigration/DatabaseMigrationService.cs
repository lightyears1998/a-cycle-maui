using ACycle.Extensions;
using ACycle.Services.DatabaseMigration;
using ACycle.Services.DatabaseMigration.Migrators;
using CommunityToolkit.Diagnostics;
using SQLite;
using System.Text;

namespace ACycle.Services
{
    public class DatabaseMigrationService : IDatabaseMigrationService
    {
        private readonly IDictionary<long, Lazy<Migrator>> _schemaMigratorMap = new Dictionary<long, Lazy<Migrator>>();
        private readonly IServiceProvider _serviceProvider;
        private readonly IStaticConfigurationService _staticConfigurationService;

        public DatabaseMigrationService(IServiceProvider serviceProvider, IStaticConfigurationService staticConfiguration)
        {
            _serviceProvider = serviceProvider;
            _staticConfigurationService = staticConfiguration;

            RegisterMigrators();
        }

        private void RegisterMigrators()
        {
            RegisterMigrator<MigratorV0ToV1>();
            RegisterMigrator<MigratorV1ToV2>();
        }

        private void RegisterMigrator<TMigrator>() where TMigrator : Migrator, new()
        {
            long sourceSchemaVersion = Migrator.GetSchemaVersions<TMigrator>().Source;

            _schemaMigratorMap.Add(sourceSchemaVersion, new Lazy<Migrator>(
                () =>
                {
                    var migrator = (TMigrator)Activator.CreateInstance(typeof(TMigrator))!;
                    Guard.IsNotNull(migrator);
                    return migrator;
                }));
        }

        public Task<string> MigrateDatabaseAsync(string migrationDatabasePath)
        {
            return MigrateDatabaseAsync(new SQLiteAsyncConnection(migrationDatabasePath));
        }

        public async Task<string> MigrateDatabaseAsync(SQLiteAsyncConnection migrationDatabase)
        {
            StringBuilder migrationResultBuilder = new();
            long? lastSchemaVersion = null;
            long? schemaVersion = await migrationDatabase.GetSchemaAsync();

            if (!schemaVersion.HasValue)
            {
                schemaVersion = 0;
            }

            var targetDatabaseSchemaVersion = _staticConfigurationService.DatabaseSchemaVersion;
            while (schemaVersion < targetDatabaseSchemaVersion && schemaVersion != lastSchemaVersion)
            {
                lastSchemaVersion = schemaVersion;

                Lazy<Migrator>? migrator;
                _schemaMigratorMap.TryGetValue(schemaVersion.Value, out migrator);

                if (migrator == null)
                {
                    break;
                }
                var migrationResult = await migrator.Value.MigrateAsync(migrationDatabase);
                migrationResultBuilder.AppendLine(migrationResult);
                await migrator.Value.PostMigrateAsync(migrationDatabase);

                schemaVersion = await migrationDatabase.GetSchemaAsync();
                if (schemaVersion == lastSchemaVersion)
                {
                    ThrowHelper.ThrowInvalidDataException("The schema version of the migrated database should be updated.");
                }
            }

            return migrationResultBuilder.ToString();
        }

        /// <summary>
        /// Merge the merging database into the base database.
        /// </summary>
        /// <param name="baseDatabase">The base database</param>
        /// <param name="mergingDatabase">The database that is being merged into the base database</param>
        public async Task<string> MergeDatabaseAsync(SQLiteAsyncConnection baseDatabase, SQLiteAsyncConnection mergingDatabase)
        {
            StringBuilder mergeResultBuilder = new();

            var databaseService = (IDatabaseService)_serviceProvider.GetService(_staticConfigurationService.DatabaseServiceImplement)!;

            mergeResultBuilder.AppendLine($"Entry count before merging: {await databaseService.CountEntriesAsync()}");
            await databaseService.MergeDatabaseAsync(mergingDatabase);
            mergeResultBuilder.AppendLine($"Entry count after merging: {await databaseService.CountEntriesAsync()}");

            return mergeResultBuilder.ToString();
        }
    }
}
