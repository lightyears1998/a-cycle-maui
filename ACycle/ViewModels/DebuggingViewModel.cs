using ACycle.Resources.Strings;
using ACycle.Services;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Localization;
using System.Windows.Input;

#if WINDOWS
using System.Diagnostics;
#endif

namespace ACycle.ViewModels
{
    public class DebuggingViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databseService;
        private readonly IConfigurationService _configurationService;
        private readonly IStringLocalizer _stringLocalizer;

        public ICommand OpenDataDirectoryCommand { get; }

        public string NodeUuidLabelText
        {
            get => $"{_stringLocalizer["Text_Node"]} UUID: {_configurationService.NodeUuid}";
        }

        public DebuggingViewModel(
            IDatabaseService databaseService,
            IConfigurationService configurationService,
            IStringLocalizer<AppStrings> stringLocalizer
        )
        {
            _databseService = databaseService;
            _configurationService = configurationService;
            _stringLocalizer = stringLocalizer;

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
