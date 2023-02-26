using ACycle.Resources.Strings;

namespace ACycle.Services
{
    public class DialogService : Service, IDialogService
    {
        public Task Prompt(string title, string message, string? confirm = null)
        {
            confirm ??= AppStrings.Text_Dialog_Confirm;

            return Application.Current!.MainPage!.DisplayAlert(title, message, confirm);
        }

        public Task<bool> Confirm(string title, string message, string? accept = null, string? cancel = null)
        {
            accept ??= AppStrings.Text_Dialog_Accept;
            cancel ??= AppStrings.Text_Dialog_Cancel;

            return Application.Current!.MainPage!.DisplayAlert(title, message, accept, cancel);
        }

        public Task<bool> ConfirmAppRestart(string message)
        {
            string title = AppStrings.Text_Dialog_ConfirmAppRestartTitle;
            string accept = AppStrings.Text_Dialog_ConfirmAppRestartAccept;
            string cancel = AppStrings.Text_Dialog_ConfirmAppRestartCancel;

            return Confirm(title, message, accept, cancel);
        }
    }
}
