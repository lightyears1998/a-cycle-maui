﻿using ACycle.Repositories;
using ACycle.Resources.Strings;
using ACycle.Services;
using ACycle.ViewModels;
using ACycle.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Localization;

#if DEBUG

using Microsoft.Extensions.Logging;

#endif

namespace ACycle
{
    public static class MauiProgram
    {
        public static StaticConfigurationService StaticConfiguration { get; } = new();

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureLocalization()
                .ConfigureLogging()
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

        public static MauiAppBuilder ConfigureLogging(this MauiAppBuilder builder)
        {
#if DEBUG
            builder.Services.AddLogging(configure =>
            {
                configure.AddDebug();
            });
#endif
            return builder;
        }

        public static MauiAppBuilder RegisterRepositories(this MauiAppBuilder builder)
        {
            builder.Services
                .AddSingleton<MetadataRepository>()
                .AddSingleton<IEntryRepository, EntryRepository>()
                .AddSingleton(typeof(IEntryRepository<>), typeof(EntryRepository<>));

            return builder;
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            builder.Services
                .AddSingleton<IAppLifecycleService, AppLifecycleService>()
                .AddTransient<IBackupService, BackupService>()
                .AddSingleton<IConfigurationService, ConfigurationService>()
                .AddSingleton(typeof(IDatabaseService), StaticConfiguration.DatabaseServiceImplement)
                .AddSingleton<IDatabaseMigrationService, DatabaseMigrationService>()
                .AddSingleton<IDialogService, DialogService>()
                .AddSingleton<IMetadataService, MetadataService>()
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<IStaticConfigurationService>(StaticConfiguration)
                .AddSingleton<ISynchronizationService, SynchronizationService>()
                .AddSingleton<ISynchronizationEndpointService, SynchronizationEndpointService>()
                .AddSingleton<IUserService, UserService>();

            builder.RegisterEntryBasedServices();

            return builder;
        }

        public static MauiAppBuilder RegisterEntryBasedServices(this MauiAppBuilder builder)
        {
            builder.Services
                .AddSingleton(typeof(IEntryService<,>), typeof(EntryService<,>))
                .AddSingleton<IActivityService, ActivityService>()
                .AddSingleton<IActivityCategoryService, ActivityCategoryService>()
                .AddSingleton<IDiaryService, DiaryService>();

            return builder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.RegisterTransients(new Type[]
            {
                typeof(ActivityViewModel),
                typeof(ActivityAnalysisViewModel),
                typeof(ActivityCategoryViewModel),
                typeof(ActivityEditorViewModel),
                typeof(DatabaseMigrationViewModel),
                typeof(DebuggingViewModel),
                typeof(DiaryViewModel),
                typeof(DiaryEditorViewModel),
                typeof(FocusViewModel),
                typeof(LandingViewModel),
                typeof(SettingsViewModel),
                typeof(SynchronizationEndpointViewModel),
                typeof(SynchronizationEndpointEditorViewModel),
            });

            return builder;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            builder.RegisterTransients(new Type[]
            {
                typeof(ActivityView),
                typeof(ActivityAnalysisView),
                typeof(ActivityCategoryView),
                typeof(ActivityEditorView),
                typeof(DatabaseMigrationView),
                typeof(DebuggingView),
                typeof(DiaryEditorView),
                typeof(DiaryView),
                typeof(FocusView),
                typeof(HealthView),
                typeof(LandingView),
                typeof(LedgerView),
                typeof(SettingsView),
                typeof(SynchronizationEndpointView),
                typeof(SynchronizationEndpointEditorView),
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
}
