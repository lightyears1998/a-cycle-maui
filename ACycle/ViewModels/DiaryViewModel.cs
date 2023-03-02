using ACycle.Entities;
using ACycle.Models;
using ACycle.Models.Base;
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

        private Diary? _selectedDiary;

        public Diary? SelectedDiary
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
                }),
                removeCommand: new AsyncRelayCommand(async () =>
                {
                    var shouldRemove = await ConfirmRemoveDiaryAsync();

                    if (shouldRemove)
                    {
                        collection.Remove(item);
                    }
                })));

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
            for (int i = 0; i < Diaries.Count; ++i)
            {
                if (Diaries[i].Item.Uuid == args.Model.Uuid)
                {
                    Diaries.NotifyItemChangedAt(i);
                }
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

            await _navigationService.NavigateToAsync("Editor", new Dictionary<string, object> { { "diary", SelectedDiary } });
        }

        [RelayCommand(CanExecute = nameof(SelectedDiaryIsNotEmpty))]
        public async Task RemoveDiaryAsync()
        {
            if (SelectedDiary == null)
                return;

            await _diaryService.RemoveAsync(SelectedDiary);
            Diaries.Remove(SelectedDiary);
        }

        public async Task<bool> ConfirmRemoveDiaryAsync()
        {
            return await _dialogService.Confirm("Confirm Remove", "Do you really want to remove this diary?");
        }

        public class DiaryRelay : Relay<Diary>
        {
            public ICommand EditCommand;

            public ICommand RemoveCommand;

            public DiaryRelay(Diary item, ICommand editCommand, ICommand removeCommand) : base(item)
            {
                EditCommand = editCommand;
                RemoveCommand = removeCommand;
            }
        }
    }
}
