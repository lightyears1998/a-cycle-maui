using ACycle.Models;
using ACycle.Models.Base;
using ACycle.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Platform;
using System.Windows.Input;

namespace ACycle.ViewModels
{
    public partial class SynchronizationEndpointViewModel : ViewModelBase
    {
        private ISynchronizationEndpointService _endpointService;
        private INavigationService _navigationService;

        [ObservableProperty]
        private RelayCollection<SynchronizationEndpoint, SynchronizationEndpointRelay> _relaySynchronizationEndpoints;

        public SynchronizationEndpointViewModel(
            ISynchronizationEndpointService endpointService,
            INavigationService navigation)
        {
            _endpointService = endpointService;
            _navigationService = navigation;

            _relaySynchronizationEndpoints = new RelayCollection<SynchronizationEndpoint, SynchronizationEndpointRelay>(
                (item, collection) => new SynchronizationEndpointRelay(item,
                editCommand: new AsyncRelayCommand(async () =>
                    {
                        await EditEndpointAsync(item);
                    }
                ),
                removeCommand: new AsyncRelayCommand(async () =>
                    {
                        await RemoveEndpointAsync(item);
                    }))
                );
        }

        public override async Task InitializeAsync()
        {
            await LoadEndpointsAsync();
        }

        public override void OnViewNavigatedTo(NavigatedToEventArgs args)
        {
            _ = LoadEndpointsAsync();
        }

        private async Task LoadEndpointsAsync()
        {
            var endpoints = await _endpointService.LoadAsync();
            RelaySynchronizationEndpoints.Reload(endpoints);
        }

        [RelayCommand]
        public async Task AddEndpointAsync()
        {
            await _navigationService.NavigateToAsync(AppShell.Route.SynchronizationEndpointEditorViewRoute);
        }

        [RelayCommand]
        public async Task EditEndpointAsync(SynchronizationEndpoint endpoint)
        {
            await _navigationService.NavigateToAsync(
                AppShell.Route.SynchronizationEndpointEditorViewRoute,
                new Dictionary<string, object> { { nameof(SynchronizationEndpointEditorViewModel.Endpoint), endpoint } });
        }

        public async Task RemoveEndpointAsync(SynchronizationEndpoint endpoint)
        {
            await _endpointService.RemoveAsync(endpoint);
            RelaySynchronizationEndpoints.Remove(endpoint);
        }

        public class SynchronizationEndpointRelay : Relay<SynchronizationEndpoint>
        {
            public ICommand EditCommand { get; }

            public ICommand RemoveCommand { get; }

            public string ShareUri => Item.PasswordSha256;

            public SynchronizationEndpointRelay(SynchronizationEndpoint item, ICommand editCommand, ICommand removeCommand) : base(item)
            {
                EditCommand = editCommand;
                RemoveCommand = removeCommand;
            }
        }
    }
}
