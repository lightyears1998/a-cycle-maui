namespace ACycle.Services
{
    public class StaticConfigurationService : Service, IStaticConfigurationService
    {
        public string AppWindowTitle
        {
#if DEBUG
            get => "ACycle (DEV)";
#else
            get => "ACycle";
#endif
        }
    }
}
