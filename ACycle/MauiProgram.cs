using ACycle.Models;
using ACycle.Repositories;
using ACycle.Services;
using ACycle.ViewModels;
using ACycle.Views;

namespace ACycle;

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
            .RegisterRepositories()
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews();

        return builder.Build();
    }

    public static MauiAppBuilder RegisterRepositories(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<EntryRepository>()
            .AddSingleton<EntryBasedModelRepository<Diary>>()
            .AddSingleton<MetadataRepository>();

        return builder;
    }

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<IDatabaseService, DatabaseService>()
            .AddSingleton<IConfigurationService, ConfigurationService>()
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<IStaticConfigurationService, StaticConfigurationService>();

        return builder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services
            .AddTransient<DebuggingViewModel>()
            .AddTransient<DiaryViewModel>()
            .AddTransient<DiaryEditorViewModel>()
            .AddTransient<FocusViewModel>()
            .AddTransient<LandingViewModel>();

        return builder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services
            .AddTransient<ActivityView>()
            .AddTransient<DebuggingView>()
            .AddTransient<DiaryEditorView>()
            .AddTransient<DiaryView>()
            .AddTransient<FocusView>()
            .AddTransient<HealthView>()
            .AddTransient<LandingView>()
            .AddTransient<LedgerView>()
            .AddTransient<SettingsView>()
            .AddTransient<TodoListView>();

        return builder;
    }
}
