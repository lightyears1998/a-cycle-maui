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
    }
}
