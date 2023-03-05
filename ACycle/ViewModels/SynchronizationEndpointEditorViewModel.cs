using ACycle.Models;
using ACycle.Resources.Strings;
using ACycle.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ACycle.ViewModels
{
    [QueryProperty(nameof(Endpoint), "Endpoint")]
    public partial class SynchronizationEndpointEditorViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly ISynchronizationEndpointService _endpointService;

        private SynchronizationEndpoint _endpoint = new();
        private SynchronizationEndpoint _lastSavedEndpoint = new();

        public SynchronizationEndpoint Endpoint
        {
            get => _endpoint;
            set
            {
                if (SetProperty(ref _endpoint, value with { }))
                {
                    _lastSavedEndpoint = _endpoint;
                }
            }
        }

        public bool HttpPortIsValid { set; get; }

        public bool WsPortIsValid { set; get; }

        [ObservableProperty]
        private bool _maskPassword = true;

        public SynchronizationEndpointEditorViewModel(
            IDialogService dialogService,
            INavigationService navigationService,
            ISynchronizationEndpointService endpointService)
        {
            _endpointService = endpointService;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        [RelayCommand]
        public void ToggleMaskPassword()
        {
            MaskPassword = !MaskPassword;
        }

        public bool EndpointIsValid => HttpPortIsValid && WsPortIsValid;

        [RelayCommand]
        public async Task SaveEndpointAsync()
        {
            if (EndpointIsValid)
            {
                await _endpointService.SaveAsync(Endpoint);
                _lastSavedEndpoint = Endpoint;
                await _navigationService.GoBackAsync();
            }
            else
            {
                await _dialogService.PromptAsync(
                    AppStrings.SynchronizationEndpointEditorView_Dialog_InvalidConfigTitle,
                    AppStrings.SynchronizationEndpointEditorView_Dialog_InvalidConfigMessage);
            }
        }

        [RelayCommand]
        public async Task ConfirmForLeaveAsync()
        {
            if (_lastSavedEndpoint != _endpoint)
            {
                await _navigationService.ConfirmForLeaveAsync();
            }
            else
            {
                await _navigationService.GoBackAsync();
            }
        }
    }
}
