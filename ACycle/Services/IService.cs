namespace ACycle.Services
{
    public interface IService
    {
        virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
