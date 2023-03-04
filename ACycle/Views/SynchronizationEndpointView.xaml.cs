using ACycle.ViewModels;

namespace ACycle.Views
{
    public partial class SynchronizationEndpointView : ContentPageBase
    {
        public SynchronizationEndpointView(SynchronizationEndpointViewModel model)
        {
            BindingContext = model;
            InitializeComponent();
        }
    }
}
