using ACycle.Models;
using ACycle.Models.Base;
using ACycle.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ACycle.ViewModels
{
    public partial class SynchronizationEndpointViewModel : ViewModelBase
    {
        private ISynchronizationEndpointService _endpointService;
        private INavigationService _navigationService;

        [ObservableProperty]
        private RelayCollection<SynchronizationEndpoint, RelaySynchronizationEndpoint> _relaySynchronizationEndpoints;

        public SynchronizationEndpointViewModel(
            ISynchronizationEndpointService endpointService,
            INavigationService navigation)
        {
            _endpointService = endpointService;
            _navigationService = navigation;

            _relaySynchronizationEndpoints = new RelayCollection<SynchronizationEndpoint, RelaySynchronizationEndpoint>(
                (item, collection) => new RelaySynchronizationEndpoint(item,
                editCommand: new AsyncRelayCommand(async () =>
                    {
                        await EditEndpoint(item);
                    }
                ),
                removeCommand: new AsyncRelayCommand(async () =>
                    {
                        await _endpointService.RemoveAsync(item);
                        collection.Remove(item);
                    }))
                );
        }

        public override async Task InitializeAsync()
        {
            await LoadEndpoints();
        }

        public override void OnViewNavigatingFrom(NavigatingFromEventArgs args)
        {
            _ = LoadEndpoints();
        }

        private async Task LoadEndpoints()
        {
            var endpoints = await _endpointService.LoadAsync();
            RelaySynchronizationEndpoints.Reload(endpoints);
        }

        [RelayCommand]
        public async Task AddEndpoint()
        {
            await _navigationService.NavigateToAsync(AppShell.Route.SynchronizationEndpointEditorViewRoute);
        }

        [RelayCommand]
        public async Task EditEndpoint(SynchronizationEndpoint endpoint)
        {
            await _navigationService.NavigateToAsync(
                AppShell.Route.SynchronizationEndpointEditorViewRoute,
                new Dictionary<string, object> { { "endpoint", endpoint } });
        }

        public class RelaySynchronizationEndpoint : Relay<SynchronizationEndpoint>
        {
            public ICommand EditCommand { get; }

            public ICommand RemoveCommand { get; }

            public RelaySynchronizationEndpoint(SynchronizationEndpoint item, ICommand editCommand, ICommand removeCommand) : base(item)
            {
                EditCommand = editCommand;
                RemoveCommand = removeCommand;
            }
        }
    }
}
