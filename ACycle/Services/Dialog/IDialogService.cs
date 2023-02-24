namespace ACycle.Services
{
    public interface IDialogService : IService
    {
        Task Prompt(string title, string message, string? confirm = null);

        Task<bool> Confirm(string title, string message, string? accept = null, string? cancel = null);

        Task<bool> ConfirmAppRestart(string message);
    }
}
