using ACycle.Services;
using ACycle.Views;

#if WINDOWS
using Microsoft.Windows.AppLifecycle;
#endif

namespace ACycle
{
    public partial class App : Application
    {
        private readonly IStaticConfigurationService _staticConfigurationService;
        private readonly IDialogService _dialogService;

        public readonly IServiceProvider ServiceProvider;

        public App(
            IServiceProvider provider,
            IStaticConfigurationService staticConfigurationService,
            IDialogService dialogService,
            LandingView landingView)
        {
            _staticConfigurationService = staticConfigurationService;
            _dialogService = dialogService;
            ServiceProvider = provider;

            MainPage = landingView;
            InitializeComponent();
        }

        public new static App? Current()
        {
            return Application.Current as App;
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

        public void Restart()
        {
#if WINDOWS
            AppInstance.Restart("");
#endif

            MainPage = new AppShell();
        }
    }
}
