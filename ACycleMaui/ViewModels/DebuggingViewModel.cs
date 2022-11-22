using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Windows.Input;

namespace ACycleMaui.ViewModels
{
    public class DebuggingViewModel
    {
        public ICommand OpenDataDirectoryCommand { get; }

        public DebuggingViewModel()
        {
            OpenDataDirectoryCommand = new AsyncRelayCommand(OpenDataDirectoryAsync);
        }

        private async Task OpenDataDirectoryAsync()
        {
#if WINDOWS
            ProcessStartInfo startInfo = new()
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
