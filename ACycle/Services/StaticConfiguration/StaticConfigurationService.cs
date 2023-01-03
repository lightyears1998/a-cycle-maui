namespace ACycle.Services
{
    public class StaticConfigurationService : IStaticConfigurationService
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
