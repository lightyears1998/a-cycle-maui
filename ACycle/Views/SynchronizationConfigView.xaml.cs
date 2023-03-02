using ACycle.ViewModels;

namespace ACycle.Views
{
    public partial class SynchronizationConfigView : ContentPageBase
    {
        public SynchronizationConfigView(SynchronizationConfigViewModel model)
        {
            BindingContext = model;
            InitializeComponent();
        }
    }
}
