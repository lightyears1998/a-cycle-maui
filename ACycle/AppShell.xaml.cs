using ACycle.Views;

namespace ACycle
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            InitializeRouting();
        }

        private static void InitializeRouting()
        {
            Routing.RegisterRoute(Route.DatabaseMigrationToolRoute, typeof(DatabaseMigrationView));
            Routing.RegisterRoute(Route.DebuggingToolRoute, typeof(DebuggingView));
            Routing.RegisterRoute(Route.DiaryEditorRoute, typeof(DiaryEditorView));
        }

        public static class Route
        {
            public static string DatabaseMigrationToolRoute = "DatabaseMigration";

            public static string DebuggingToolRoute = "Debugging";

            public static string DiaryEditorRoute = "Diary/Editor";
        }
    }
}
