using ACycle.Entities;
using ACycle.Models;
using ACycle.Services;
using CommunityToolkit.Mvvm.Input;

namespace ACycle.ViewModels
{
    [QueryProperty(nameof(Diary), "diary")]
    public partial class DiaryEditorViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEntryService<DiaryV1, Diary> _diaryService;

        private Diary _diary = new();

        public Diary Diary
        {
            get => _diary;
            set => SetProperty(ref _diary, value);
        }

        public DateTime DiaryDate
        {
            get => Diary.DateTime.Date;
            set
            {
                if (DiaryDate != value)
                {
                    Diary.DateTime = value + Diary.DateTime.TimeOfDay;
                    OnPropertyChanged(nameof(DiaryDate));
                }
            }
        }

        public TimeSpan DiaryTime
        {
            get => Diary.DateTime.TimeOfDay;
            set
            {
                if (DiaryTime != value)
                {
                    Diary.DateTime = Diary.DateTime.Date + value;
                    OnPropertyChanged(nameof(DiaryTime));
                }
            }
        }

        public DiaryEditorViewModel(INavigationService navigationService, IEntryService<DiaryV1, Diary> diaryService)
        {
            _navigationService = navigationService;
            _diaryService = diaryService;
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            await _diaryService.SaveAsync(Diary);
            await _navigationService.PopAsync();
        }

        [RelayCommand]
        public async Task DiscardAsync()
        {
            await _navigationService.PopAsync();
        }
    }
}
