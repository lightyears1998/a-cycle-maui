using ACycle.ViewModels;

namespace ACycle.Views
{
    public partial class ActivityView : ContentPageBase
    {
        public ActivityView(ActivityViewModel model)
        {
            BindingContext = model;
            InitializeComponent();
        }
    }
}
