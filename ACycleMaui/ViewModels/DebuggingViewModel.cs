using ACycle.AppServices;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Windows.Input;

namespace ACycleMaui.ViewModels
{
    public class DebuggingViewModel
    {
        private readonly IDatabaseService _databseService;
        private readonly IConfigurationService _configurationService;

        public ICommand OpenDataDirectoryCommand { get; }

        public string NodeUuidLabelText
        {
            get => $"Node UUID: {_configurationService.NodeUuid}";
        }

        public DebuggingViewModel(IDatabaseService databaseService, IConfigurationService configurationService)
        {
            _databseService = databaseService;
            _configurationService = configurationService;

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
