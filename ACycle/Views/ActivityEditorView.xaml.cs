using ACycle.ViewModels;

namespace ACycle.Views
{
    public partial class ActivityEditorView : ContentPageBase
    {
        public ActivityEditorView(ActivityEditorViewModel model)
        {
            BindingContext = model;
            InitializeComponent();
        }
    }
}
