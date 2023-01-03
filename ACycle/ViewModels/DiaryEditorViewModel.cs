using ACycle.Models;
using ACycle.Repositories;
using ACycle.Services;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ACycle.ViewModels
{
    public class DiaryEditorViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly EntryBasedModelRepository<Diary> _diaryRepository;

        public Diary Diary { set; get; } = new();

        public ICommand SaveCommand { get; }

        public ICommand DiscardCommand { get; }

        public DiaryEditorViewModel(INavigationService navigationService, EntryBasedModelRepository<Diary> diaryRepository)
        {
            _navigationService = navigationService;
            _diaryRepository = diaryRepository;

            SaveCommand = new AsyncRelayCommand(SaveAsync);
            DiscardCommand = new AsyncRelayCommand(DiscardAsync);
        }

        private async Task SaveAsync()
        {
            await _diaryRepository.SaveAsync(Diary);
        }

        private async Task DiscardAsync()
        {
            await _navigationService.PopAsync();
        }
    }
}
