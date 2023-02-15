namespace ACycle.Services
{
    public class Service : IService
    {
        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
