using ACycle.ViewModels;

namespace ACycle.Views;

public partial class ActivityCategoryView : ContentPageBase
{
    public ActivityCategoryView(ActivityCategoryViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
