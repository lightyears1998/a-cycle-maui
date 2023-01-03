namespace ACycle.Services
{
    public interface IStaticConfigurationService : IService
    {
        string AppWindowTitle { get; }
    }
}
