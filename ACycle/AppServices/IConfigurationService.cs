namespace ACycle.AppServices
{
    public interface IConfigurationService : IAppService
    {
        Guid NodeUuid { set; get; }
    }
}
