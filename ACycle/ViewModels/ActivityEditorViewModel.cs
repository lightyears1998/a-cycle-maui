using ACycle.Entities;
using ACycle.Models;
using ACycle.Services;
using CommunityToolkit.Mvvm.Input;

namespace ACycle.ViewModels
{
    [QueryProperty(nameof(Activity), "Activity")]
    public partial class ActivityEditorViewModel : ViewModelBase
    {
        private Activity _activity = new();
        private Activity _lastSavedActivity = new();

        private INavigationService _navigationService;
        private IEntryService<ActivityV1, Activity> _activityService;

        public bool ActivityHasActivity => _activity != _lastSavedActivity;

        public Activity Activity
        {
            get => _activity;
            set => SetProperty(ref _activity, value with { });
        }

        public ActivityEditorViewModel(
            INavigationService navigationService,
            IEntryService<ActivityV1, Activity> activityService)
        {
            _navigationService = navigationService;
            _activityService = activityService;
        }

        [RelayCommand]
        public async Task ConfirmForLeave()
        {
            if (ActivityHasActivity)
            {
                await _navigationService.ConfirmForLeaveAsync();
            }
            else
            {
                await _navigationService.GoBackAsync();
            }
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            await _activityService.SaveAsync(Activity);
            _lastSavedActivity = _activity with { };
            await _navigationService.GoBackAsync();
        }

        [RelayCommand]
        public async Task DiscardAsync()
        {
            await ConfirmForLeave();
        }
    }
}
