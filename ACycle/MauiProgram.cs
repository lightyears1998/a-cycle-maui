using ACycle.Repositories;
using ACycle.Resources.Strings;
using ACycle.Services;
using ACycle.ViewModels;
using ACycle.Views;
using Microsoft.Extensions.Localization;

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
            .ConfigureLocalization()
            .RegisterRepositories()
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews();

        return builder.Build();
    }

    public static MauiAppBuilder ConfigureLocalization(this MauiAppBuilder builder)
    {
        builder.Services
            .AddLocalization()
            .AddTransient<IStringLocalizer, StringLocalizer<AppStrings>>();

        return builder;
    }

    public static MauiAppBuilder RegisterRepositories(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<MetadataRepository>()
            .AddSingleton(typeof(IEntryRepository<>), typeof(EntryRepository<>));

        return builder;
    }

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton(typeof(IEntryService<,>), typeof(EntryService<,>));

        builder.Services
            .AddSingleton<IActivityCategoryService, ActivityCategoryService>()
            .AddSingleton<IConfigurationService, ConfigurationService>()
            .AddSingleton<IDatabaseService>(new DatabaseService(FileSystem.AppDataDirectory))
            .AddSingleton<IDialogService, DialogService>()
            .AddSingleton<IMetadataService, MetadataService>()
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<IStaticConfigurationService, StaticConfigurationService>()
            .AddSingleton<IUserService, UserService>();

        return builder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services
            .AddTransient<DebuggingViewModel>()
            .AddTransient<DiaryViewModel>()
            .AddTransient<DiaryEditorViewModel>()
            .AddTransient<FocusViewModel>()
            .AddTransient<LandingViewModel>()
            .AddTransient<SettingsViewModel>();

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
            .AddTransient<PlanningView>();

        return builder;
    }
}
