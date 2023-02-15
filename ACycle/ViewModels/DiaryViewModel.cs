using ACycle.Entities;
using ACycle.Models;
using ACycle.Services;
using CommunityToolkit.Mvvm.Input;

namespace ACycle.ViewModels
{
    public partial class DiaryViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEntryService<DiaryV1, Diary> _diaryService;

        private DateTime _date = DateTime.Today;
        private ObservableCollectionEx<Diary> _diaries = new();
        private Diary? _selectedDiary;

        public DateTime Date
        {
            get => _date;
            set
            {
                if (SetProperty(ref _date, value))
                {
                    Task.Run(LoadDiaries);
                }
            }
        }

        public ObservableCollectionEx<Diary> Diaries
        {
            get => _diaries;
            set => SetProperty(ref _diaries, value);
        }

        public Diary? SelectedDiary
        {
            get => _selectedDiary;
            set => SetProperty(ref _selectedDiary, value);
        }

        public DiaryViewModel(INavigationService navigationService, IEntryService<DiaryV1, Diary> diaryService)
        {
            _navigationService = navigationService;
            _diaryService = diaryService;
            _diaryService.ModelCreated += OnDiaryCreated;
            _diaryService.ModelUpdated += OnDiaryUpdated;
            _diaryService.ModelRemoved += OnDiaryRemoved;
        }

        private void OnDiaryCreated(object? sender, EntryServiceEventArgs<Diary> args)
        {
            if (args.Model.DateTime.Date == Date)
            {
                Diaries.InsertSorted(args.Model, Comparer<Diary>.Create((a, b) => a.DateTime.CompareTo(b.DateTime)));
            }
        }

        private void OnDiaryUpdated(object? sender, EntryServiceEventArgs<Diary> args)
        {
            if (Diaries.Contains(args.Model))
            {
                Diaries.NotifyItemChangedAt(Diaries.IndexOf(args.Model));
            }
        }

        private void OnDiaryRemoved(object? sender, EntryServiceEventArgs<Diary> args)
        {
            if (Diaries.Contains(args.Model))
            {
                Diaries.Remove(args.Model);
            }
        }

        public override async Task InitializeAsync()
        {
            await IsBusyFor(LoadDiaries);
        }

        private async Task<IEnumerable<Diary>> GetDiariesOfTheDate(DateTime date)
        {
            var diaries = await _diaryService.FindAllAsync();
            return diaries
                .Where(diary => DateOnly.FromDateTime(diary.DateTime) == DateOnly.FromDateTime(date))
                .OrderBy(diary => diary.DateTime);
        }

        private async Task LoadDiaries()
        {
            Diaries.Reload(await GetDiariesOfTheDate(Date));
        }

        [RelayCommand]
        private void JumpToPreviousDate()
        {
            Date = Date.AddDays(-1);
        }

        [RelayCommand]
        private void JumpToNextDate()
        {
            Date = Date.AddDays(1);
        }

        [RelayCommand]
        private async Task OpenEditorForAdding()
        {
            await _navigationService.NavigateToAsync("Editor");
        }

        [RelayCommand]
        private async Task OpenEditorForEditing()
        {
            if (SelectedDiary == null)
                return;

            await _navigationService.NavigateToAsync("Editor", new Dictionary<string, object> { { "diary", SelectedDiary } });
        }

        [RelayCommand]
        private async Task RemoveDiary()
        {
            if (SelectedDiary == null)
                return;

            await _diaryService.RemoveAsync(SelectedDiary);
            Diaries.Remove(SelectedDiary);
        }
    }
}
