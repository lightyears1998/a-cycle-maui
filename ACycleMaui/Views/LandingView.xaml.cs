using ACycle.AppServices;

namespace ACycleMaui.Views;

public partial class LandingView : ContentPage
{
    private readonly IDatabaseService _databaseService;

    public LandingView(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
        InitializeComponent();
        InitializeAndThenNavigateToAppShell();
    }

    private async void InitializeAndThenNavigateToAppShell()
    {
        await InitializeAppServices();
        NavigateToAppShell();
    }

    private async Task InitializeAppServices()
    {
        await _databaseService.Initialize();
    }

    private static void NavigateToAppShell()
    {
        Application.Current!.MainPage = new AppShell();
    }
}
