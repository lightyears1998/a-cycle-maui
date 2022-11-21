using ACycle.AppServices;
using ACycle.AppServices.Impl;
using System.Runtime.CompilerServices;

namespace ACycleMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .RegisterAppServices()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        return builder.Build();
    }

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
    {
        IAppService[] services = {
            new DatabaseService()
        };

        foreach (var service in services)
        {
            builder.Services.AddSingleton(service);
        }

        return builder;
    }
}
