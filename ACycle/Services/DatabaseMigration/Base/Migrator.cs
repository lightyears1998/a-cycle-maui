using ACycle.Extensions;
using CommunityToolkit.Diagnostics;
using SQLite;

namespace ACycle.Services.DatabaseMigration
{
    public abstract class Migrator
    {
        public long SourceSchemaVersion { get; }

        public long DestinationSchemaVersion { get; }

        public Migrator()
        {
            var schemaVersions = GetSchemaVersions(this);
            SourceSchemaVersion = schemaVersions.Source;
            DestinationSchemaVersion = schemaVersions.Destination;
        }

        public static MigratorSchemaVersionAttribute GetSchemaVersions<TMigrator>() where TMigrator : Migrator
        {
            return GetSchemaVersions(typeof(TMigrator));
        }

        public static MigratorSchemaVersionAttribute GetSchemaVersions<TMigrator>(TMigrator migrator) where TMigrator : Migrator
        {
            return GetSchemaVersions(migrator.GetType());
        }

        private static MigratorSchemaVersionAttribute GetSchemaVersions(Type type)
        {
            var attributes = Attribute.GetCustomAttributes(type);
            foreach (var attribute in attributes)
            {
                if (attribute is MigratorSchemaVersionAttribute migratorAttribute)
                {
                    return migratorAttribute;
                }
            }

            ThrowHelper.ThrowMissingFieldException($"Migrator attributes are not set. " +
                $"Apply {nameof(MigratorSchemaVersionAttribute)} to migrator class to set attributes.");
            return null;
        }

        public abstract Task<string> MigrateAsync(SQLiteAsyncConnection connection);

        public virtual async Task PostMigrateAsync(SQLiteAsyncConnection connection)
        {
            await connection.SetSchemaAsync(DestinationSchemaVersion);
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MigratorSchemaVersionAttribute : Attribute
    {
        public long Source;

        public long Destination;
    }
}
