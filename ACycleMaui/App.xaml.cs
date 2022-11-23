using ACycleMaui.Views;

namespace ACycleMaui;

public partial class App : Application
{
    public App(LandingView landingView)
    {
        InitializeComponent();
        MainPage = landingView;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        if (window != null)
        {
            window.Title = "ACycle";
        }

        return window!;
    }
}
