﻿using ACycle.Models;
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
            .RegisterEntityRepository()
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews();

        return builder.Build();
    }

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<IDatabaseService, DatabaseService>()
            .AddSingleton<IConfigurationService, ConfigurationService>()
            .AddSingleton<INavigationService, NavigationService>();

        return builder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services
            .AddTransient<DebuggingViewModel>()
            .AddTransient<DiaryViewModel>()
            .AddTransient<DiaryEditorViewModel>();

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

    public static MauiAppBuilder RegisterEntityRepository(this MauiAppBuilder builder)
    {
        builder.Services
            .AddTransient<EntryRepository>()
            .AddTransient<EntryBasedModelRepository<Diary>>()
            .AddTransient<MetadataRepository>();

        return builder;
    }
}
