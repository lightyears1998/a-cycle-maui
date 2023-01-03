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

        public ICommand JumpToPreviousDateCommand { get; }

        public ICommand JumpToNextDateCommand { get; }

        public ICommand OpenEditorCommand { get; }

        public DiaryViewModel(INavigationService navigationService, EntryBasedModelRepository<Diary> diaryRepository)
        {
            _navigationService = navigationService;
            _diaryRepository = diaryRepository;

            JumpToPreviousDateCommand = new AsyncRelayCommand(JumpToPreviousDate);
            JumpToNextDateCommand = new AsyncRelayCommand(JumpToNextDate);
            OpenEditorCommand = new AsyncRelayCommand(OpenEditor);
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
            return diaries.Where(diary => DateOnly.FromDateTime(diary.Date) == DateOnly.FromDateTime(date));
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

        private async Task OpenEditor()
        {
            await _navigationService.NavigateToAsync("Editor");
        }
    }
}
