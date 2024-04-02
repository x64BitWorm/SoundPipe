using Microsoft.Extensions.DependencyInjection;
using SP.Domain;
using SP.UI.Services;
using SP.UI.ViewModels;
using SP.UI.Views;
using System;
using System.Windows;

namespace SP.UI
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var settings = _serviceProvider.GetService<SettingsProvider>().Load();
            _serviceProvider.GetService<FiltersManager>()?.LoadFilters(settings.FiltersPath);
            _serviceProvider.GetService<MainWindow>()?.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainWindow>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsWindow>();
            services.AddSingleton<ShemeProvider>();
            services.AddSingleton<ContextMenuBuilder>();
            services.AddSingleton<ShemeGraphConverter>();
            services.AddSingleton<PropertiesViewManager>();
            services.AddSingleton<SettingsProvider>();
            MainModule.RegisterServices(services);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
