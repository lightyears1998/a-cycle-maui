using ACycle.Entities;
using ACycle.Models;
using ACycle.Models.Base;
using ACycle.Resources.Strings;
using ACycle.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ACycle.ViewModels
{
    public partial class ActivityViewModel : ViewModelBase
    {
        private IEntryService<ActivityV1, Activity> _activityService;
        private INavigationService _navigationService;
        private IDialogService _dialogService;

        private DateTime _date = DateTime.Today;

        public DateTime Date
        {
            get => _date; set
            {
                if (SetProperty(ref _date, value))
                {
                    _ = LoadActivitiesAsync();
                }
            }
        }

        [ObservableProperty]
        private RelayCollection<Activity, ActivityRelay> _activities;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedActivityIsNotNull))]
        private ActivityRelay? _selectedActivity;

        public bool SelectedActivityIsNotNull => SelectedActivity != null;

        public ActivityViewModel(
            IEntryService<ActivityV1, Activity> activityService,
            INavigationService navigation,
            IDialogService dialogService)
        {
            _activityService = activityService;
            _navigationService = navigation;
            _dialogService = dialogService;

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

        public override void OnViewNavigatedTo(NavigatedToEventArgs args)
        {
            _ = LoadActivitiesAsync();
        }

        private async Task LoadActivitiesAsync()
        {
            Activities.Reload(await GetActivitiesOfTheDate(Date));
        }

        private async Task<IEnumerable<Activity>> GetActivitiesOfTheDate(DateTime date)
        {
            var activities = await _activityService.FindAllAsync();
            return activities
                .Where(activity =>
                    DateOnly.FromDateTime(activity.StartDateTime) <= DateOnly.FromDateTime(date) &&
                    DateOnly.FromDateTime(activity.EndDateTime) >= DateOnly.FromDateTime(date))
                .OrderBy(activity => activity.StartDateTime);
        }

        [RelayCommand]
        public async Task AddActivityAsync()
        {
            await OpenEditor(null);
        }

        [RelayCommand(CanExecute = nameof(SelectedActivityIsNotNull))]
        public async Task EditActivityAsync()
        {
            if (SelectedActivity != null)
            {
                await EditActivityAsync(SelectedActivity.Item);
            }
        }

        public async Task EditActivityAsync(Activity activity)
        {
            await OpenEditor(activity);
        }

        [RelayCommand(CanExecute = nameof(SelectedActivityIsNotNull))]
        public async Task RemoveActivityAsync()
        {
            if (SelectedActivity != null)
            {
                await RemoveActivityAsync(SelectedActivity.Item);
            }
        }

        public async Task RemoveActivityAsync(Activity activity)
        {
            var shouldRemove = await ConfirmRemoveActivityAsync();
            if (shouldRemove)
            {
                await _activityService.RemoveAsync(activity);
                await LoadActivitiesAsync();
            }
        }

        public async Task<bool> ConfirmRemoveActivityAsync()
        {
            return await _dialogService.RequestAsync(AppStrings.ActivityView_ConfirmRemoveTitle, AppStrings.ActivityView_ConfirmRemoveText);
        }

        private async Task OpenEditor(Activity? activity)
        {
            await _navigationService.NavigateToAsync(
                AppShell.Route.ActivityEditorViewRoute,
                activity == null ? null : new Dictionary<string, object> { { "Activity", activity } });
        }

        public class ActivityRelay : Relay<Activity>
        {
            public ICommand EditCommand { get; protected set; }

            public ICommand RemoveCommand { get; protected set; }

            public string DateTimeString => $"{Item.StartDateTime} => {Item.EndDateTime}";

            public ActivityRelay(Activity item, ICommand editCommand, ICommand removeCommand) : base(item)
            {
                EditCommand = editCommand;
                RemoveCommand = removeCommand;
            }
        }
    }
}
