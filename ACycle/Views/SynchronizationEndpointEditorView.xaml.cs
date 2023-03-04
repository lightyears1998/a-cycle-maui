using ACycle.ViewModels;

namespace ACycle.Views
{
    public partial class SynchronizationEndpointEditorView : ContentPageBase
    {
        public SynchronizationEndpointEditorView(SynchronizationEndpointEditorViewModel model)
        {
            BindingContext = model;
            InitializeComponent();
        }
    }
}
