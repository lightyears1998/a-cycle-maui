using ACycle.Models;
using ACycle.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ACycle.ViewModels
{
    [QueryProperty(nameof(Activity), "Activity")]
    public partial class ActivityEditorViewModel : ViewModelBase
    {
        private Activity _activity = new();
        private Activity _lastSavedActivity = new();

        [ObservableProperty]
        private ObservableCollectionEx<CategoryItem> _categoryItems = new() { CategoryItem.NullItem };

        [ObservableProperty]
        private CategoryItem _selectedCategory = CategoryItem.NullItem;

        private readonly INavigationService _navigationService;
        private readonly IActivityService _activityService;
        private readonly IActivityCategoryService _categoryService;

        public bool ActivityHasActivity => _activity != _lastSavedActivity;

        public Activity Activity
        {
            get => _activity;
            set => SetProperty(ref _activity, value with { });
        }

        public ActivityEditorViewModel(
            INavigationService navigationService,
            IActivityService activityService,
            IActivityCategoryService categoryService)
        {
            _navigationService = navigationService;
            _activityService = activityService;
            _categoryService = categoryService;
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

        [RelayCommand]
        public async Task OpenCategoryView()
        {
            await _navigationService.NavigateToAsync(AppShell.Route.ActivityCategoryViewRoute);
        }

        public record class CategoryItem
        {
            public string Name { set; get; }

            public ActivityCategory? Category { set; get; }

            public static readonly CategoryItem NullItem = new CategoryItem("无", null);

            public CategoryItem(string name, ActivityCategory? category)
            {
                Name = name;
                Category = category;
            }
        }
    }
}
