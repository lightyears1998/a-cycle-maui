using ACycleMaui.ViewModels;

namespace ACycleMaui.Views;

public partial class DiaryView : ContentPage
{
    public DiaryView(DiaryViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}
