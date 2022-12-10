namespace ACycle.AppServices
{
    public interface IAppService
    {
        virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
