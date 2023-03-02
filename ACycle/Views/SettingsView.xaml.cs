using ACycle.ViewModels;

namespace ACycle.Views
{
    public partial class SettingsView : ContentPageBase
    {
        public SettingsView(SettingsViewModel model)
        {
            BindingContext = model;
            InitializeComponent();
        }
    }
}
