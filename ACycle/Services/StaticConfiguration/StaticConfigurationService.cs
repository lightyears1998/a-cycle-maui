using ACycle.Resources.Strings;
using Microsoft.Extensions.Localization;

namespace ACycle.Services
{
    public class StaticConfigurationService : Service, IStaticConfigurationService
    {
        private readonly IStringLocalizer<AppStrings> _stringLocalizer;

        public StaticConfigurationService(IStringLocalizer<AppStrings> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        public string AppWindowTitle
        {
            get
            {
                string title = _stringLocalizer["AppName"];
#if DEBUG
                title += " (DEV)";
#endif
                return title;
            }
        }
    }
}
