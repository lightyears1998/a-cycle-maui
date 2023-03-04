using ACycle.Entities;
using ACycle.Models;
using ACycle.Resources.Strings;
using ACycle.Services;
using CommunityToolkit.Mvvm.Input;

namespace ACycle.ViewModels
{
    [QueryProperty(nameof(Diary), "Diary")]
    public partial class DiaryEditorViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEntryService<DiaryV1, Diary> _diaryService;
        private readonly IDialogService _dialogService;

        private Diary _lastSavedDiary = new();
        private Diary _diary = new();

        public bool DiaryHasChanged => _diary.Content != _lastSavedDiary.Content || _diary.Title != _lastSavedDiary.Title;

        public Diary Diary
        {
            get => _diary;
            set
            {
                if (SetProperty(ref _diary, value with { }))
                {
                    OnPropertyChanged(nameof(DiaryDate));
                    OnPropertyChanged(nameof(DiaryTime));
                }
            }
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

        public DiaryEditorViewModel(INavigationService navigationService, IEntryService<DiaryV1, Diary> diaryService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _diaryService = diaryService;
            _dialogService = dialogService;
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _lastSavedDiary = _diary with { };
        }

        [RelayCommand]
        public async Task ConfirmForLeaveAsync()
        {
            if (DiaryHasChanged)
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
            await _diaryService.SaveAsync(Diary);
            _lastSavedDiary = _diary with { };
            await _navigationService.GoBackAsync();
        }

        [RelayCommand]
        public async Task DiscardAsync()
        {
            await ConfirmForLeaveAsync();
        }
    }
}
