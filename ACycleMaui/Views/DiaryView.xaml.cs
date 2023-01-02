using ACycleMaui.ViewModels;

namespace ACycleMaui.Views;

public partial class DiaryView : ContentPageBase
{
    public DiaryView(DiaryViewModel model)
    {
        BindingContext = model;
        InitializeComponent();
    }
}
