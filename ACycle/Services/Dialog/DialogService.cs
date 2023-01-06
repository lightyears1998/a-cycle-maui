namespace ACycle.Services
{
    public class DialogService : Service, IDialogService
    {
        public Task Prompt(string title, string message, string confirm)
        {
            return Application.Current!.MainPage!.DisplayAlert(title, message, confirm);
        }

        public Task<bool> Confirm(string title, string message, string accept, string cancel)
        {
            return Application.Current!.MainPage!.DisplayAlert(title, message, accept, cancel);
        }
    }
}
