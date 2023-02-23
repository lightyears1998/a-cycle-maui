using ACycle.ViewModels;

namespace ACycle.Views
{
    public partial class DatabaseMigrationView : ContentPageBase
    {
        public DatabaseMigrationView(DatabaseMigrationViewModel model)
        {
            BindingContext = model;
            InitializeComponent();
        }
    }
}