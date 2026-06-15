using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.IO;
using System.Windows;
using VitaTest.AppCore;
using VitaTest.AppCore.Services;
using VitaTest.AppCore.Services.Interfaces;
using VitaTest.Infrastructure.Database;
using VitaTest.Infrastructure.Interfaces;
using VitaTest.Infrastructure.Services;
using VitaTest.Modules.Incomes;
using VitaTest.Modules.Orders;
using VitaTest.Modules.Payments;
using VitaTest.Modules.Settings;
using VitaTest.Views;

namespace VitaTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<OrdersModule>();
            moduleCatalog.AddModule<PaymentsModule>();
            moduleCatalog.AddModule<IncomesModule>();
            moduleCatalog.AddModule<SettingsModule>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var settings = ConfigureAppSettings(containerRegistry);
            ConfigureDatabase(containerRegistry, settings);

            var notifiesQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
            containerRegistry.RegisterInstance<ISnackbarMessageQueue>(notifiesQueue);
            containerRegistry.RegisterSingleton<INotificationService, SnackBarNotificationService>();
            containerRegistry.RegisterSingleton<IDataUpdateService, DataUpdateService>();
        }

        private AppSettings ConfigureAppSettings(IContainerRegistry containerRegistry)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                                                          .Build();

            containerRegistry.RegisterInstance<IConfiguration>(configuration);

            var settings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(settings);

            containerRegistry.RegisterInstance(settings);
            containerRegistry.RegisterInstance(Options.Create(settings));
            containerRegistry.RegisterSingleton<ISettingsService, SettingsService>();

            return settings;
        }

        private void ConfigureDatabase(IContainerRegistry containerRegistry, AppSettings settings)
        {
            containerRegistry.RegisterSingleton<IDbContextFactory<VitaDatabase>>(() =>
            {
                var containerExtension = containerRegistry as IContainerExtension;
                var settingsService = containerExtension?.Resolve<ISettingsService>();

                if (settingsService == null)
                    throw new InvalidOperationException("Не удалось получить SettingsService");

                return new VitaDatabaseFactory(settingsService, BuildConnectionString);
            });
        }

        private string BuildConnectionString(AppSettings settings)
        {
            return $"Server={settings.DbAddress};" +
                   $"Database={settings.DbName};" +
                   $"{(settings.LocalDb ? string.Empty : $"User Id={settings.DbLogin}")};" +
                   $"{(settings.LocalDb ? string.Empty : $"Password={settings.DbPassword}")};" +
                   $"{(settings.LocalDb ? "TrustServerCertificate=True;" : string.Empty)}" +
                   $"{(settings.LocalDb ? "Integrated Security=True;" : string.Empty)}";
        }
    }
}
