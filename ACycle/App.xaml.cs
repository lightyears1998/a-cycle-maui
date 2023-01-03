using ACycle.Services;
using ACycle.Views;

namespace ACycle;

public partial class App : Application
{
    private readonly IStaticConfigurationService _staticConfigurationService;

    public App(LandingView landingView, IStaticConfigurationService staticConfigurationService)
    {
        _staticConfigurationService = staticConfigurationService;
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
