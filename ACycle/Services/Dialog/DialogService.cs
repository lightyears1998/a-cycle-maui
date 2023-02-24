using ACycle.Resources.Strings;
using Microsoft.Extensions.Localization;

namespace ACycle.Services
{
    public class DialogService : Service, IDialogService
    {
        private readonly IStringLocalizer<AppStrings> _stringLocalizer;

        public DialogService(IStringLocalizer<AppStrings> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        public Task Prompt(string title, string message, string? confirm = null)
        {
            confirm ??= _stringLocalizer["Text_Dialog_Confirm"];

            return Application.Current!.MainPage!.DisplayAlert(title, message, confirm);
        }

        public Task<bool> Confirm(string title, string message, string? accept = null, string? cancel = null)
        {
            accept ??= _stringLocalizer["Text_Dialog_Accept"];
            cancel ??= _stringLocalizer["Text_Dialog_Cancel"];

            return Application.Current!.MainPage!.DisplayAlert(title, message, accept, cancel);
        }

        public Task<bool> ConfirmAppRestart(string message)
        {
            string title = _stringLocalizer["Text_Dialog_ConfirmAppRestartTitle"];
            string accept = _stringLocalizer["Text_Dialog_ConfirmAppRestartAccept"];
            string cancel = _stringLocalizer["Text_Dialog_ConfirmAppRestartCancel"];

            return Confirm(title, message, accept, cancel);
        }
    }
}
