﻿using ACycle.Repositories;
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
        builder.RegisterTransients(new Type[]
        {
            typeof(DebuggingViewModel),
            typeof(DiaryViewModel),
            typeof(DiaryEditorViewModel),
            typeof(FocusViewModel),
            typeof(LandingViewModel),
            typeof(SettingsViewModel),
        });

        return builder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.RegisterTransients(new Type[]
        {
            typeof(ActivityView),
            typeof(DebuggingView),
            typeof(DiaryEditorView),
            typeof(DiaryView),
            typeof(FocusView),
            typeof(HealthView),
            typeof(LandingView),
            typeof(LedgerView),
            typeof(SettingsView),
            typeof(PlanningView)
        });

        return builder;
    }

    public static MauiAppBuilder RegisterTransients(this MauiAppBuilder builder, IList<Type> types)
    {
        foreach (var type in types)
        {
            builder.Services.AddTransient(type);
        }

        return builder;
    }
}
