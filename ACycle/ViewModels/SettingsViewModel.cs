using ACycle.Helpers;
using ACycle.Services;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace ACycle.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IConfigurationService _configurationService;
        private readonly IUserService _userService;

        private List<string> _supportedLanguageDisplayNames = new();
        private int _selectedLanguageIndex = 0;
        private bool _restartDueToLanguageChangeHintIsVisible = false;
        private string _restartDueToLanguageChangeHint = "";

        public List<CultureInfo?> SupportedLanguages { get; } = new List<CultureInfo?> {
            null,
            new CultureInfo("en"),
            new CultureInfo("zh-Hans")
        };

        public List<string> SupportedLanguageDisplayNames
        {
            get => _supportedLanguageDisplayNames;
            set => SetProperty(ref _supportedLanguageDisplayNames, value);
        }

        public int SelectedLanguageIndex
        {
            get => _selectedLanguageIndex;
            set
            {
                if (SetProperty(ref _selectedLanguageIndex, value))
                {
                    LanguageHelper.SwitchLanguage(SelectedLanguage);
                    GetSupportedLanguageDisplayNames();
                    Task.Run(UpdateLanguageSettings);
                    ShowRestartDueToLanguageChangeHint();
                    _ = ConfirmAppRestart();
                }
            }
        }

        public CultureInfo SelectedLanguage => SupportedLanguages[_selectedLanguageIndex] ?? _configurationService.SystemCultureInfo;

        public bool RestartDueToLanguageChangeHintIsVisible
        {
            get => _restartDueToLanguageChangeHintIsVisible;
            set => SetProperty(ref _restartDueToLanguageChangeHintIsVisible, value);
        }

        public string RestartDueToLanguageChangeHint
        {
            get => _restartDueToLanguageChangeHint;
            set => SetProperty(ref _restartDueToLanguageChangeHint, value);
        }

        public SettingsViewModel(
            IStringLocalizer stringLocalizer,
            IDialogService dialogService,
            INavigationService navigationService,
            IConfigurationService configurationService,
            IUserService userService)
        {
            _stringLocalizer = stringLocalizer;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _configurationService = configurationService;
            _userService = userService;

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
            string systemLanguageDisplayName = _stringLocalizer["SettingsView_UseSystemLanguage"];
            _supportedLanguageDisplayNames = SupportedLanguages.Select(language => language?.NativeName ?? systemLanguageDisplayName).ToList();
        }

        private async Task UpdateLanguageSettings()
        {
            var userInfo = await _userService.GetUserInfoAsync();
            userInfo.PreferredLanguage = SelectedLanguage;
            await _userService.SaveUserInfoAsync(userInfo);
        }

        private void ShowRestartDueToLanguageChangeHint()
        {
            RestartDueToLanguageChangeHintIsVisible = true;
            RestartDueToLanguageChangeHint = _stringLocalizer["SettingsView_RestartDueToLanguageChangeHint"];
        }

        [RelayCommand]
        public async Task OpenDebuggingMenuAsync()
        {
            await _navigationService.NavigateToAsync("/Debugging");
        }

        private async Task ConfirmAppRestart()
        {
            var shouldRestart = await _dialogService.ConfirmAppRestart(_stringLocalizer["Text_AppRestartReason_LanguageChanges"]);
            if (shouldRestart)
            {
                App.Current()!.Restart();
            }
        }
    }
}
