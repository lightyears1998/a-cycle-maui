using ACycle.Resources.Strings;

namespace ACycle.Services
{
    public class DialogService : Service, IDialogService
    {
        public Task PromptAsync(string title, string message, string? confirm = null)
        {
            confirm ??= AppStrings.Text_Dialog_Confirm;

            return Application.Current!.MainPage!.DisplayAlert(title, message, confirm);
        }

        public Task<bool> RequestAsync(string title, string message, string? accept = null, string? cancel = null)
        {
            accept ??= AppStrings.Text_Dialog_Accept;
            cancel ??= AppStrings.Text_Dialog_Cancel;

            return Application.Current!.MainPage!.DisplayAlert(title, message, accept, cancel);
        }
    }
}
