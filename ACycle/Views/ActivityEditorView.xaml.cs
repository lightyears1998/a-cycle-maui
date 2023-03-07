using ACycle.Models;
using ACycle.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

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
