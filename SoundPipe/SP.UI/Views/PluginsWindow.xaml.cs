using SP.UI.ViewModels;
using System.Windows;

namespace SP.UI.Views
{
    public partial class PluginsWindow : Window
    {
        private readonly PluginsViewModel _viewModel;

        public PluginsWindow(PluginsViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.ViewAttached();
        }

        private void SourceCodeClick(object sender, RoutedEventArgs e)
        {
            _viewModel.OpenSourceCode();
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            _viewModel.DeletePlugin();
        }
    }
}
