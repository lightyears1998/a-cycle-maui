using ACycle.ViewModels;

namespace ACycle.Views
{
    public partial class ActivityAnalysisView : ContentPageBase
    {
        public ActivityAnalysisView(ActivityAnalysisViewModel model)
        {
            BindingContext = model;
            InitializeComponent();
        }
    }
}
