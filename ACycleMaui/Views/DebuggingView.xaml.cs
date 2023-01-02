using ACycleMaui.ViewModels;

namespace ACycleMaui.Views;

public partial class DebuggingView : ContentPageBase
{
    public DebuggingView(DebuggingViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
