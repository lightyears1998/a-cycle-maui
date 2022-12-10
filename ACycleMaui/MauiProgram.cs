using ACycle.AppServices;
using ACycle.AppServices.Impl;
using ACycle.EntityRepositories;
using ACycle.Models;
using ACycleMaui.ViewModels;
using ACycleMaui.Views;

namespace ACycleMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterEntityRepository()
            .RegisterAppServices()
            .RegisterViewModels()
            .RegisterViews();

        return builder.Build();
    }

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<IDatabaseService, DatabaseService>()
            .AddSingleton<IConfigurationService, ConfigurationService>();

        return builder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<DebuggingViewModel>();

        return builder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services
            .AddTransient<AcitivityView>()
            .AddTransient<DebuggingView>()
            .AddTransient<FocusView>()
            .AddTransient<LandingView>()
            .AddTransient<SettingsView>();

        return builder;
    }

    public static MauiAppBuilder RegisterEntityRepository(this MauiAppBuilder builder)
    {
        builder.Services
            .AddTransient<EntryRepository>()
            .AddTransient<EntryRepository<Diary>>()
            .AddTransient<MetadataRepository>();

        return builder;
    }
}
