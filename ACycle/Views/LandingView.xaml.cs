using ACycle.Services;

namespace ACycle.Views;

public partial class LandingView : ContentPageBase
{
    private readonly IDatabaseService _databaseService;
    private readonly IConfigurationService _configurationService;

    public LandingView(
        IDatabaseService databaseService,
        IConfigurationService configurationService
    )
    {
        _databaseService = databaseService;
        _configurationService = configurationService;

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
        await _databaseService.InitializeAsync();
        await _configurationService.InitializeAsync();
    }

    private static void NavigateToAppShell()
    {
        Application.Current!.MainPage = new AppShell();
    }
}
