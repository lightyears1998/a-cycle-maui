namespace ACycle.Services
{
    public interface IStaticConfigurationService : IService
    {
        string AppWindowTitle { get; }

        string MainDatabasePath { get; }

        Type DatabaseServiceImplement { get; }

        long DatabaseSchemaVersion { get; }
    }
}
