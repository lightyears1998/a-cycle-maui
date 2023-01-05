namespace ACycle.Services
{
    public interface IDialogService : IService
    {
        Task<bool> Confirm(string title, string message, string accept, string cancel);

        Task Prompt(string title, string message, string confirm);
    }
}
