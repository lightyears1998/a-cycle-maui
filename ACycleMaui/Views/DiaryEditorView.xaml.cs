using ACycleMaui.ViewModels;

namespace ACycleMaui.Views;

public partial class DiaryEditorView : ContentPageBase
{
    public DiaryEditorView(DiaryEditorViewModel model)
    {
        BindingContext = model;
        InitializeComponent();
    }
}
