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
            Routing.RegisterRoute(Route.DatabaseMigrationViewRoute, typeof(DatabaseMigrationView));
            Routing.RegisterRoute(Route.DebuggingViewRoute, typeof(DebuggingView));
            Routing.RegisterRoute(Route.DiaryEditorViewRoute, typeof(DiaryEditorView));
            Routing.RegisterRoute(Route.SynchronizationEndpointViewRoute, typeof(SynchronizationEndpointView));
            Routing.RegisterRoute(Route.SynchronizationEndpointEditorViewRoute, typeof(SynchronizationEndpointEditorView));
        }

        public static class Route
        {
            public const string DatabaseMigrationViewRoute = "DatabaseMigration";

            public const string DebuggingViewRoute = "Debugging";

            public const string DiaryEditorViewRoute = "Diary/Editor";

            public const string SynchronizationEndpointViewRoute = "SynchronizationEndpoint";

            public const string SynchronizationEndpointEditorViewRoute = "SynchronizationEndpoint/Editor";
        }
    }
}
