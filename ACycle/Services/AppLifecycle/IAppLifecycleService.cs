namespace ACycle.Services
{
    public interface IAppLifecycleService
    {
        Task<bool> RequestAppRestart(string message);
    }
}
