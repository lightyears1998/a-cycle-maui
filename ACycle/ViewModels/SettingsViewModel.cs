using ACycle.Helpers;
using ACycle.Resources.Strings;
using ACycle.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;

namespace ACycle.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        private readonly IAppLifecycleService _appLifecycleService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IConfigurationService _configurationService;
        private readonly IUserService _userService;
        private readonly ISynchronizationService _synchronizationService;

        public List<CultureInfo?> SupportedLanguages { get; } = new List<CultureInfo?> {
            null,
            new CultureInfo("en"),
            new CultureInfo("zh-Hans")
        };

        private List<string> _supportedLanguageDisplayNames = new();

        public List<string> SupportedLanguageDisplayNames
        {
            get => _supportedLanguageDisplayNames;
            set => SetProperty(ref _supportedLanguageDisplayNames, value);
        }

        private int _selectedLanguageIndex = 0;

        public int SelectedLanguageIndex
        {
            get => _selectedLanguageIndex;
            set
            {
                if (SetProperty(ref _selectedLanguageIndex, value))
                {
                    LanguageHelper.SwitchLanguage(SelectedLanguage);
                    GetSupportedLanguageDisplayNames();
                    Task.Run(UpdateLanguageSettingsAsync);
                    ShowRestartDueToLanguageChangeHint();
                    _ = ConfirmAppRestartAsync();
                }
            }
        }

        public CultureInfo SelectedLanguage => SupportedLanguages[_selectedLanguageIndex] ?? _configurationService.SystemCultureInfo;

        private bool _restartDueToLanguageChangeHintIsVisible = false;

        public bool RestartDueToLanguageChangeHintIsVisible
        {
            get => _restartDueToLanguageChangeHintIsVisible;
            set => SetProperty(ref _restartDueToLanguageChangeHintIsVisible, value);
        }

        private string _restartDueToLanguageChangeHint = "";

        public string RestartDueToLanguageChangeHint
        {
            get => _restartDueToLanguageChangeHint;
            set => SetProperty(ref _restartDueToLanguageChangeHint, value);
        }

        public bool SynchronizationEnabled
        {
            get => _synchronizationService.SynchronizationEnabled;
            set
            {
                SynchronizationSwitchEnabled = false;
                _synchronizationService.SetSynchronizationEnabledAsync(value).ContinueWith((_) =>
                {
                    SynchronizationSwitchEnabled = true;
                });
            }
        }

        [ObservableProperty]
        private bool _synchronizationSwitchEnabled = true;

        public string SynchronizationStatus => _synchronizationService.SynchronizationStatus;

        public SettingsViewModel(
            IAppLifecycleService appLifecycleService,
            IDialogService dialogService,
            INavigationService navigationService,
            IConfigurationService configurationService,
            IUserService userService,
            ISynchronizationService synchronizationService)
        {
            _appLifecycleService = appLifecycleService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _configurationService = configurationService;
            _userService = userService;
            _synchronizationService = synchronizationService;

            GetSupportedLanguageDisplayNames();
        }

        public override async Task InitializeAsync()
        {
            var userInfo = await _userService.GetUserInfoAsync();

            if (userInfo.PreferredLanguage == null)
            {
                SetProperty(ref _selectedLanguageIndex, 0, nameof(SelectedLanguageIndex));
            }
            else
            {
                var selectedLanguageIndex = Math.Max(SupportedLanguages.FindIndex(userInfo.PreferredLanguage.Equals), 0);
                SetProperty(
                    ref _selectedLanguageIndex,
                    selectedLanguageIndex,
                    nameof(SelectedLanguageIndex));
            }
        }

        private void GetSupportedLanguageDisplayNames()
        {
            string systemLanguageDisplayName = AppStrings.SettingsView_UseSystemLanguage;
            _supportedLanguageDisplayNames = SupportedLanguages.Select(language => language?.NativeName ?? systemLanguageDisplayName).ToList();
        }

        private async Task UpdateLanguageSettingsAsync()
        {
            var userInfo = await _userService.GetUserInfoAsync();
            userInfo.PreferredLanguage = SelectedLanguage;
            await _userService.SaveUserInfoAsync(userInfo);
        }

        private void ShowRestartDueToLanguageChangeHint()
        {
            RestartDueToLanguageChangeHintIsVisible = true;
            RestartDueToLanguageChangeHint = AppStrings.SettingsView_RestartDueToLanguageChangeHint;
        }

        [RelayCommand]
        public async Task OpenEndpointListViewAsync()
        {
            await _navigationService.NavigateToAsync(AppShell.Route.SynchronizationEndpointViewRoute);
        }

        [RelayCommand]
        public async Task DoSync()
        {
            await _synchronizationService.SyncAsync();
            OnPropertyChanged(nameof(SynchronizationStatus));
        }

        [RelayCommand]
        public async Task OpenDebuggingMenuAsync()
        {
            await _navigationService.NavigateToAsync(AppShell.Route.DebuggingViewRoute);
        }

        private async Task ConfirmAppRestartAsync()
        {
            var shouldRestart = await _appLifecycleService.RequestAppRestart(AppStrings.Text_AppRestartReason_LanguageChanges);
            if (shouldRestart)
            {
                App.Current()!.Restart();
            }
        }
    }
}
