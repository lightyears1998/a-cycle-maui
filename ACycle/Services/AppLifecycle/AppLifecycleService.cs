using ACycle.Resources.Strings;

namespace ACycle.Services
{
    public class AppLifecycleService : Service, IAppLifecycleService
    {
        private readonly IDialogService _dialogService;

        public AppLifecycleService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public Task<bool> RequestAppRestart(string message)
        {
            string title = AppStrings.Text_Dialog_RequestAppRestartTitle;
            string accept = AppStrings.Text_Dialog_RequestAppRestartAccept;
            string cancel = AppStrings.Text_Dialog_RequestAppRestartCancel;

            return _dialogService.RequestAsync(title, message, accept, cancel);
        }
    }
}
