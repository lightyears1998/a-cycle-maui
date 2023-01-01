namespace ACycle.Services
{
    public interface IAppService
    {
        virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
