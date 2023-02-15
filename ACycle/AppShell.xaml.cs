using ACycle.Views;

namespace ACycle;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        InitializeRouting();
    }

    private static void InitializeRouting()
    {
        Routing.RegisterRoute("Debugging", typeof(DebuggingView));
        Routing.RegisterRoute("Diary/Editor", typeof(DiaryEditorView));
    }
}
