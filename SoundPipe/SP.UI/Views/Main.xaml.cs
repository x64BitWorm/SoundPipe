using SP.UI.Components.GraphViewer.Events;
using SP.UI.Components.PropertiesStore;
using SP.UI.Utils;
using SP.UI.ViewModels;
using System;
using System.Windows;

namespace SP.UI
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel, IServiceProvider provider)
        {
            _viewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        private void GraphViewerNodeClick(object sender, RoutedEventArgs e)
        {
            var nodeId = (e as ParametrizedRoutedEventArgs).Parameter.ToString();
            _viewModel.NodePropertiesOpening(nodeId);
        }

        private void GraphViewerContextOpening(object sender, RoutedEventArgs e)
        {
            var args = (e as ParametrizedRoutedEventArgs).Parameter as object[];
            var nodeId = args[0] as string;
            if (nodeId == string.Empty)
            {
                _viewModel.DefaultMenuOpening((System.Drawing.Point)args[1]);
            }
            else
            {
                _viewModel.NodeMenuOpening(nodeId);
            }
        }

        private void MainPanelLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.ViewAttached();
        }

        private void StorePropertyChange(object sender, RoutedEventArgs e)
        {
            var arguments = (e as ParametrizedRoutedEventArgs).Parameter as ChangeEventParameter;
            _viewModel.PropertyChangedEvent(arguments.Id, arguments.Value);
        }

        #region MenuItems

        private void MenuFileNewClick(object sender, RoutedEventArgs e)
        {
            _viewModel.CreateEmptyShemeMenu();
        }

        private void MenuFileOpenClick(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadShemeFromFile();
        }

        private void MenuFileSaveClick(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveShemeToFile(false);
        }

        private void MenuFileSaveAsClick(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveShemeToFile(true);
        }

        private void MenuRunClick(object sender, RoutedEventArgs e)
        {
            _viewModel.RunSheme();
        }

        private void MenuStopClick(object sender, RoutedEventArgs e)
        {
            _viewModel.StopSheme();
        }

        private void MenuStatsClick(object sender, RoutedEventArgs e)
        {
            _viewModel.ShowShemeStats();
        }

        #endregion

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_viewModel.IsShemeSaved)
            {
                var answer = MessageBox.Show("Изменения в текущей схеме будут потеряны, продолжить?",
                    "Есть несохраненные изменения", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
            _viewModel.StopSheme();
        }
    }
}
