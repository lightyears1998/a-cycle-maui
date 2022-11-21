using ACycle.AppServices;
using System.Diagnostics;

namespace ACycleMaui.Pages
{
    public partial class DebuggingPage : ContentPage
    {
        public DebuggingPage()
        {
            InitializeComponent();
        }

        public async void OnShowAppDataDirectoryButtonClicked(object sender, EventArgs _args)
        {
#if WINDOWS
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
#else
            await Task.CompletedTask; // Do nothing.
#endif
        }
    }
}
