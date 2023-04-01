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
        private readonly IEntryService<ActivityV1, Activity> _activityService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

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
        [NotifyCanExecuteChangedFor(nameof(EditActivityCommand))]
        [NotifyCanExecuteChangedFor(nameof(RemoveActivityCommand))]
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

            _activities = new RelayCollection<Activity, ActivityRelay>((item, relay) => new ActivityRelay(item, relay =>
            {
                return (
                    new RelayCommand(() => _ = EditActivityAsync(relay.Item)),
                    new RelayCommand(() => _ = RemoveActivityAsync(relay.Item))
                    );
            }));
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

        [RelayCommand]
        private async Task GoToAnalysisView()
        {
            await _navigationService.NavigateToAsync(AppShell.Route.ActivityAnalysisViewRoute);
        }

        public class ActivityRelay : Relay<Activity>
        {
            public ICommand EditCommand { get; set; }

            public ICommand RemoveCommand { get; set; }

            public string DateTimeString => $"{Item.StartDateTime} => {Item.EndDateTime} ({TimeSpan.FromTicks(Item.EndDateTime.Ticks - Item.StartDateTime.Ticks).TotalHours:0.##}hr)";

            public ActivityRelay(Activity item, Func<ActivityRelay, (ICommand EditCommand, ICommand RemoveCommand)> commandBuilder) : base(item)
            {
                var commands = commandBuilder(this);
                EditCommand = commands.EditCommand;
                RemoveCommand = commands.RemoveCommand;
            }
        }
    }
}
