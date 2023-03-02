using ACycle.Repositories;
using ACycle.Resources.Strings;
using ACycle.Services;
using ACycle.ViewModels;
using ACycle.Views;
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
                .AddSingleton(typeof(IEntryRepository<>), typeof(EntryRepository<>));

            return builder;
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            builder.Services
                .AddSingleton<IActivityCategoryService, ActivityCategoryService>()
                .AddTransient<IBackupService, BackupService>()
                .AddSingleton<IConfigurationService, ConfigurationService>()
                .AddSingleton(typeof(IDatabaseService), StaticConfiguration.DatabaseServiceImplement)
                .AddSingleton(typeof(IEntryService<,>), typeof(EntryService<,>))
                .AddSingleton<IDatabaseMigrationService, DatabaseMigrationService>()
                .AddSingleton<IDialogService, DialogService>()
                .AddSingleton<IMetadataService, MetadataService>()
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<IStaticConfigurationService>(StaticConfiguration)
                .AddSingleton<ISynchronizationService, SynchronizationService>()
                .AddSingleton<ISynchronizationEndpointService, SynchronizationEndpointService>()
                .AddSingleton<IUserService, UserService>();

            return builder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.RegisterTransients(new Type[]
            {
                typeof(DatabaseMigrationViewModel),
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
                typeof(DatabaseMigrationView),
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
}
