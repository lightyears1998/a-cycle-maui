using ACycle.ViewModels;

namespace ACycle.Views;

public partial class DiaryEditorView : ContentPageBase
{
    public DiaryEditorView(DiaryEditorViewModel model)
    {
        BindingContext = model;
        InitializeComponent();
    }
}
