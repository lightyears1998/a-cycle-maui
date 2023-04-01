using ACycle.Views;
using System.Security;

namespace ACycle
{
    public partial class AppShell : Shell
    {
        public bool ShowDebuggingFlyoutItem
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        public AppShell()
        {
            BindingContext = this;
            InitializeComponent();
            InitializeRouting();
        }

        private static void InitializeRouting()
        {
            Routing.RegisterRoute(Route.ActivityAnalysisViewRoute, typeof(ActivityAnalysisView));
            Routing.RegisterRoute(Route.ActivityCategoryViewRoute, typeof(ActivityCategoryView));
            Routing.RegisterRoute(Route.ActivityEditorViewRoute, typeof(ActivityEditorView));
            Routing.RegisterRoute(Route.DatabaseMigrationViewRoute, typeof(DatabaseMigrationView));
            Routing.RegisterRoute(Route.DebuggingViewRoute, typeof(DebuggingView));
            Routing.RegisterRoute(Route.DiaryEditorViewRoute, typeof(DiaryEditorView));
            Routing.RegisterRoute(Route.SynchronizationEndpointViewRoute, typeof(SynchronizationEndpointView));
            Routing.RegisterRoute(Route.SynchronizationEndpointEditorViewRoute, typeof(SynchronizationEndpointEditorView));
        }

        public static class Route
        {
            public const string ActivityAnalysisViewRoute = "Activity/Analysis";

            public const string ActivityCategoryViewRoute = "Activity/Category";

            public const string ActivityEditorViewRoute = "Activity/Editor";

            public const string DatabaseMigrationViewRoute = "DatabaseMigration";

            public const string DebuggingViewRoute = "Debugging";

            public const string DiaryEditorViewRoute = "Diary/Editor";

            public const string SynchronizationEndpointViewRoute = "SynchronizationEndpoint";

            public const string SynchronizationEndpointEditorViewRoute = "SynchronizationEndpoint/Editor";
        }
    }
}
