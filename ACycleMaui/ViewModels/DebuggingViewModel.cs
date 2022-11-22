using ACycle.AppServices;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Windows.Input;

namespace ACycleMaui.ViewModels
{
    public class DebuggingViewModel
    {
        private IDatabaseService _databseService;

        public ICommand OpenDataDirectoryCommand { get; }

        public DebuggingViewModel(IDatabaseService databaseService)
        {
            _databseService = databaseService;

            OpenDataDirectoryCommand = new AsyncRelayCommand(OpenDataDirectoryAsync);
        }

        private async Task OpenDataDirectoryAsync()
        {
#if WINDOWS
            var startInfo = new ProcessStartInfo()
            {
                Arguments = FileSystem.AppDataDirectory,
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);

            await Task.Delay(3000);
#else
            await Task.CompletedTask;
#endif
        }
    }
}
