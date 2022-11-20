using System.Diagnostics;

namespace ACycleMaui.Pages;

public partial class DebuggingPage : ContentPage
{
    public DebuggingPage()
    {
        InitializeComponent();
    }

#if WINDOWS
    public async void OnOpenProgramDataDirectoryInExplorerButtonClicked(object sender, EventArgs _args)
    {
        Button button = (Button)sender;
        button.IsEnabled = false;

        ProcessStartInfo startInfo = new()
        {
            Arguments = FileSystem.AppDataDirectory,
            FileName = "explorer.exe"
        };
        Process.Start(startInfo);

        await Task.Delay(3000);
        button.IsEnabled = true;
    }
#endif
}
