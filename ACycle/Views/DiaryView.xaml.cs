using ACycle.ViewModels;

namespace ACycle.Views
{
    public partial class DiaryView : ContentPageBase
    {
        public DiaryView(DiaryViewModel model)
        {
            BindingContext = model;
            InitializeComponent();
        }
    }
}
