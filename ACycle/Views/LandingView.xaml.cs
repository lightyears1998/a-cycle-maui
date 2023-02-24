using ACycle.ViewModels;

namespace ACycle.Views;

public partial class LandingView : ContentPageBase
{
    public LandingView(LandingViewModel model)
    {
        BindingContext = model;
        InitializeComponent();
    }
}
