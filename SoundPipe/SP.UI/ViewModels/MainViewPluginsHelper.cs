using Microsoft.Extensions.DependencyInjection;
using SP.UI.Views;

namespace SP.UI.ViewModels
{
    partial class MainViewModel
    {
        public void ShowSettingsWindow()
        {
            _serviceProvider.GetService<SettingsWindow>()?.ShowDialog();
        }
    }
}
