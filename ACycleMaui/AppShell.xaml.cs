using ACycleMaui.Views;

namespace ACycleMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        InitializeRouting();
    }

    private static void InitializeRouting()
    {
        Routing.RegisterRoute("Diary/Editor", typeof(DiaryEditorView));
    }
}
