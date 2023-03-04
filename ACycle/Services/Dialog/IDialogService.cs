namespace ACycle.Services
{
    public interface IDialogService : IService
    {
        Task PromptAsync(string title, string message, string? confirm = null);

        Task<bool> RequestAsync(string title, string message, string? accept = null, string? cancel = null);

        Task<bool> RequestAppRestart(string message);
    }
}
