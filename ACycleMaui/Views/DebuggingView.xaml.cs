using ACycleMaui.ViewModels;
using ACycle.Models;

namespace ACycleMaui.Views;

public partial class DebuggingView : ContentPage
{
    public DebuggingView(DebuggingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
