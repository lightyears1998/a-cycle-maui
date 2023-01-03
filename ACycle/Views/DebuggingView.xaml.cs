using ACycle.ViewModels;

namespace ACycle.Views;

public partial class DebuggingView : ContentPageBase
{
    public DebuggingView(DebuggingViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
