namespace ACycle.Services
{
    public interface IConfigurationService : IAppService
    {
        Guid NodeUuid { set; get; }
    }
}
