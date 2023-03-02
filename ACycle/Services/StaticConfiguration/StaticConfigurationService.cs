using ACycle.Resources.Strings;

namespace ACycle.Services
{
    public class StaticConfigurationService : Service, IStaticConfigurationService
    {
        public string AppWindowTitle
        {
            get
            {
                string title = AppStrings.AppName;
#if DEBUG
                title += " (DEV)";
#endif
                return title;
            }
        }

        public Type DatabaseServiceImplement => typeof(DatabaseServiceV2);

        public long DatabaseSchemaVersion => DatabaseServiceBase.GetSchemaVersionOfDatabaseService(DatabaseServiceImplement);
    }
}
