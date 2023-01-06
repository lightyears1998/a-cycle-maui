using ACycle.Services;
using ACycle.Views;

namespace ACycle;

public partial class App : Application
{
    private readonly IStaticConfigurationService _staticConfigurationService;

    public readonly IServiceProvider ServiceProvider;

    public App(IServiceProvider provider, IStaticConfigurationService staticConfigurationService, LandingView landingView)
    {
        _staticConfigurationService = staticConfigurationService;
        ServiceProvider = provider;

        MainPage = landingView;
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        if (window != null)
        {
            window.Title = _staticConfigurationService.AppWindowTitle;
        }

        return window!;
    }
}
