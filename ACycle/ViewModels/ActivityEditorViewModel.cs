using ACycle.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ACycle.ViewModels
{
    public partial class ActivityEditorViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Activity _activity = new();

        [RelayCommand]
        public async Task ConfirmForLeave()
        {
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
        }

        [RelayCommand]
        public async Task DiscardAsync()
        {
        }
    }
}
