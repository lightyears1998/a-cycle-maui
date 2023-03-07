using ACycle.Entities.Activity;
using ACycle.Models;
using ACycle.Models.Base;
using ACycle.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ACycle.ViewModels
{
    public partial class ActivityViewModel : ViewModelBase
    {
        private IEntryService<ActivityV1, Activity> _activityService;

        [ObservableProperty]
        private DateTime _date = DateTime.Today;

        [ObservableProperty]
        private RelayCollection<Activity, ActivityRelay> _activities;

        [ObservableProperty]
        private ActivityRelay? _selectedActivity;

        public ActivityViewModel(IEntryService<ActivityV1, Activity> activityService)
        {
            _activityService = activityService;

            _activities = new RelayCollection<Activity, ActivityRelay>((item, _) => new ActivityRelay(
                item: item,
                editCommand: new AsyncRelayCommand(async () => await EditActivityAsync(item)),
                removeCommand: new AsyncRelayCommand(async () => await RemoveActivityAsync(item))
            ));
        }

        [RelayCommand]
        public void JumpToPreviousDate()
        {
            Date = Date.AddDays(-1);
        }

        [RelayCommand]
        public void JumpToNextDate()
        {
            Date = Date.AddDays(1);
        }

        [RelayCommand]
        public async Task AddActivityAsync()
        {
            if (SelectedActivity != null)
            {
                await AddActivityAsync(SelectedActivity.Item);
            }
        }

        public async Task AddActivityAsync(Activity activity)
        {
        }

        [RelayCommand]
        public async Task EditActivityAsync()
        {
            if (SelectedActivity != null)
            {
                await EditActivityAsync(SelectedActivity.Item);
            }
        }

        public async Task EditActivityAsync(Activity activity)
        {
        }

        [RelayCommand]
        public async Task RemoveActivityAsync()
        {
            if (SelectedActivity != null)
            {
                await RemoveActivityAsync(SelectedActivity.Item);
            }
        }

        public async Task RemoveActivityAsync(Activity activity)
        {
        }

        public class ActivityRelay : Relay<Activity>
        {
            public ICommand EditCommand { get; protected set; }

            public ICommand RemoveCommand { get; protected set; }

            public ActivityRelay(Activity item, ICommand editCommand, ICommand removeCommand) : base(item)
            {
                EditCommand = editCommand;
                RemoveCommand = removeCommand;
            }
        }
    }
}
