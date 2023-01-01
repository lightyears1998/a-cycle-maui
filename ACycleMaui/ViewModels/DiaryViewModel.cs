using ACycle.Models;
using ACycleMaui.Services;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ACycleMaui.ViewModels
{
    public class DiaryViewModel
    {
        public List<Diary> Diaries { get; protected set; } = new();

        public ICommand OpenEditorCommand { get; set; }

        private readonly INavigationService _navigationService;

        public DiaryViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            OpenEditorCommand = new AsyncRelayCommand(OpenEditor);
        }

        private async Task OpenEditor()
        {
            await _navigationService.NavigateToAsync("Editor");
        }
    }
}
