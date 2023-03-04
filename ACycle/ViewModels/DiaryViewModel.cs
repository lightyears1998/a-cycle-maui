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
    public partial class DiaryViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly IEntryService<DiaryV1, Diary> _diaryService;
        private readonly INavigationService _navigationService;

        private DateTime _date = DateTime.Today;

        public DateTime Date
        {
            get => _date;
            set
            {
                if (SetProperty(ref _date, value))
                {
                    Task.Run(LoadDiariesAsync);
                }
            }
        }

        [ObservableProperty]
        private RelayCollection<Diary, DiaryRelay> _diaries;

        private DiaryRelay? _selectedDiary;

        public DiaryRelay? SelectedDiary
        {
            get => _selectedDiary;
            set
            {
                if (SetProperty(ref _selectedDiary, value))
                {
                    OnPropertyChanged(nameof(SelectedDiaryIsNotEmpty));
                    RemoveDiaryCommand.NotifyCanExecuteChanged();
                    OpenEditorForEditingCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public bool SelectedDiaryIsNotEmpty => SelectedDiary != null;

        public DiaryViewModel(
            IDialogService dialogService,
            IEntryService<DiaryV1, Diary> diaryService,
            INavigationService navigationService)
        {
            _dialogService = dialogService;
            _diaryService = diaryService;
            _navigationService = navigationService;

            _diaries = new((item, collection) => new DiaryRelay(
                item,
                editCommand: new Command(() =>
                {
                    _ = OpenEditorForEditingAsync(item);
                }),
                removeCommand: new Command(() =>
                {
                    _ = RemoveDiaryAsync(item);
                })));

            _diaryService.ModelCreated += OnDiaryCreated;
            _diaryService.ModelUpdated += OnDiaryUpdated;
            _diaryService.ModelRemoved += OnDiaryRemoved;
        }

        private void OnDiaryCreated(object? sender, EntryServiceEventArgs<Diary> args)
        {
            if (args.Entry.DateTime.Date == Date)
            {
                Diaries.InsertSorted(args.Entry, Comparer<Diary>.Create((a, b) => a.DateTime.CompareTo(b.DateTime)));
            }
        }

        private void OnDiaryUpdated(object? sender, EntryServiceEventArgs<Diary> args)
        {
            for (int i = 0; i < Diaries.Count; ++i)
            {
                if (Diaries[i].Item.Uuid == args.Entry.Uuid)
                {
                    Diaries[i].Item = args.Entry;
                    Diaries.NotifyItemChangedAt(i);
                }
            }
        }

        private void OnDiaryRemoved(object? sender, EntryServiceEventArgs<Diary> args)
        {
            if (Diaries.Contains(args.Entry))
            {
                Diaries.Remove(args.Entry);
            }
        }

        public override async Task InitializeAsync()
        {
            await IsBusyFor(LoadDiariesAsync);
        }

        private async Task<IEnumerable<Diary>> GetDiariesOfTheDateAsync(DateTime date)
        {
            var diaries = await _diaryService.FindAllAsync();
            return diaries
                .Where(diary => DateOnly.FromDateTime(diary.DateTime) == DateOnly.FromDateTime(date))
                .OrderBy(diary => diary.DateTime);
        }

        private async Task LoadDiariesAsync()
        {
            Diaries.Reload(await GetDiariesOfTheDateAsync(Date));
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
        public async Task OpenEditorForAddingAsync()
        {
            await _navigationService.NavigateToAsync("Editor");
        }

        [RelayCommand(CanExecute = nameof(SelectedDiaryIsNotEmpty))]
        public async Task OpenEditorForEditingAsync()
        {
            if (SelectedDiary == null)
                return;

            await OpenEditorForEditingAsync(SelectedDiary.Item);
        }

        public async Task OpenEditorForEditingAsync(Diary diary)
        {
            await _navigationService.NavigateToAsync(
                AppShell.Route.DiaryEditorViewRoute,
                new Dictionary<string, object> { { "Diary", diary } });
        }

        [RelayCommand(CanExecute = nameof(SelectedDiaryIsNotEmpty))]
        public async Task RemoveDiaryAsync()
        {
            if (SelectedDiary == null)
                return;

            await RemoveDiaryAsync(SelectedDiary.Item);
        }

        public async Task RemoveDiaryAsync(Diary diary)
        {
            var shouldRemove = await ConfirmRemoveDiaryAsync();
            if (shouldRemove)
            {
                await _diaryService.RemoveAsync(diary);
                Diaries.Remove(diary);
            }
        }

        public async Task<bool> ConfirmRemoveDiaryAsync()
        {
            return await _dialogService.RequestAsync(AppStrings.DiaryView_ConfirmRemoveTitle, AppStrings.DiaryView_ConfirmRemoveText);
        }

        public class DiaryRelay : Relay<Diary>
        {
            public ICommand EditCommand { get; set; }

            public ICommand RemoveCommand { get; set; }

            public DiaryRelay(Diary item, ICommand editCommand, ICommand removeCommand) : base(item)
            {
                EditCommand = editCommand;
                RemoveCommand = removeCommand;
            }
        }
    }
}
