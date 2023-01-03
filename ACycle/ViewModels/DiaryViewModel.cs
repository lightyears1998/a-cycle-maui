using ACycle.Models;
using ACycle.Repositories;
using ACycle.Services;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ACycle.ViewModels
{
    public class DiaryViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly EntryBasedModelRepository<Diary> _diaryRepository;

        private DateTime _date = DateTime.Today;
        private IEnumerable<Diary> _diaries = new List<Diary>();
        private Diary? _selectedDiary;

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public IEnumerable<Diary> Diaries
        {
            get => _diaries;
            set => SetProperty(ref _diaries, value);
        }

        public Diary? SelectedDiary
        {
            get => _selectedDiary;
            set => SetProperty(ref _selectedDiary, value);
        }

        public ICommand JumpToPreviousDateCommand { get; }

        public ICommand JumpToNextDateCommand { get; }

        public ICommand OpenEditorForAddingCommand { get; }

        public ICommand OpenEditorForEditingCommand { get; }

        public ICommand RemoveDiaryCommand { get; }

        public DiaryViewModel(INavigationService navigationService, EntryBasedModelRepository<Diary> diaryRepository)
        {
            _navigationService = navigationService;
            _diaryRepository = diaryRepository;

            JumpToPreviousDateCommand = new AsyncRelayCommand(JumpToPreviousDate);
            JumpToNextDateCommand = new AsyncRelayCommand(JumpToNextDate);
            OpenEditorForAddingCommand = new AsyncRelayCommand(OpenEditorForAdding);
            OpenEditorForEditingCommand = new AsyncRelayCommand(OpenEditorForEditing);
            RemoveDiaryCommand = new AsyncRelayCommand(RemoveDiary);
        }

        public override async Task InitializeAsync()
        {
            await IsBusyFor(async () =>
            {
                Diaries = await GetDiariesOfTheDate(Date);
            });
        }

        private async Task<IEnumerable<Diary>> GetDiariesOfTheDate(DateTime date)
        {
            var diaries = await _diaryRepository.FindAllAsync();
            return diaries.Where(diary => DateOnly.FromDateTime(diary.DateTime) == DateOnly.FromDateTime(date));
        }

        private async Task JumpToPreviousDate()
        {
            Date = Date.AddDays(-1);
            Diaries = await GetDiariesOfTheDate(Date);
        }

        private async Task JumpToNextDate()
        {
            Date = Date.AddDays(1);
            Diaries = await GetDiariesOfTheDate(Date);
        }

        private async Task OpenEditorForAdding()
        {
            await _navigationService.NavigateToAsync("Editor");
        }

        private async Task OpenEditorForEditing()
        {
            if (SelectedDiary == null)
                return;

            await _navigationService.NavigateToAsync("Editor", new Dictionary<string, object> { { "diary", SelectedDiary } });
        }

        private async Task RemoveDiary()
        {
            if (SelectedDiary == null)
                return;

            await _diaryRepository.RemoveAsync(SelectedDiary);
            await GetDiariesOfTheDate(Date);
        }
    }
}
