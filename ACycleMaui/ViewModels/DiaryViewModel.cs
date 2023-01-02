using ACycle.Models;
using ACycleMaui.Services;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ACycleMaui.ViewModels
{
    public class DiaryViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        private DateTime _date = DateTime.Today;

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public List<Diary> Diaries { get; protected set; } = new();

        public ICommand JumpToPreviousDateCommand { get; }

        public ICommand JumpToNextDateCommand { get; }

        public ICommand OpenEditorCommand { get; }

        public DiaryViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            JumpToPreviousDateCommand = new Command(JumpToPreviousDate);
            JumpToNextDateCommand = new Command(JumpToNextDate);
            OpenEditorCommand = new AsyncRelayCommand(OpenEditor);
        }

        private void JumpToPreviousDate()
        {
            Date = Date.AddDays(-1);
        }

        private void JumpToNextDate()
        {
            Date = Date.AddDays(1);
        }

        private async Task OpenEditor()
        {
            await _navigationService.NavigateToAsync("Editor");
        }
    }
}
