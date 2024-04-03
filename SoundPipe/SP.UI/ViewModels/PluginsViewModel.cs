using SP.Domain;
using SP.UI.Models;
using SP.UI.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;

namespace SP.UI.ViewModels
{
    public class PluginsViewModel: INotifyPropertyChanged
    {
        private readonly FiltersManager _filtersManager;
        private readonly FilterUpdateService _filterUpdateService;

        private ObservableCollection<PluginInfoRow> _plugins;
        private PluginInfoRow _selectedPlugin;
        private FlowDocument _aboutDocument;

        public ObservableCollection<PluginInfoRow> Plugins
        {
            get => _plugins;
            set
            {
                _plugins = value;
                OnPropertyChanged(nameof(Plugins));
            }
        }

        public PluginInfoRow SelectedPlugin
        {
            get => _selectedPlugin;
            set
            {
                _selectedPlugin = value;
                OnPropertyChanged(nameof(SelectedPlugin));
                PluginSelectionChanged();
            }
        }

        public FlowDocument AboutDocument
        {
            get => _aboutDocument;
            set
            {
                _aboutDocument = value;
                OnPropertyChanged(nameof(AboutDocument));
            }
        }

        public PluginsViewModel(FiltersManager filtersManager, FilterUpdateService filterUpdateService)
        {
            _filtersManager = filtersManager;
            _filterUpdateService = filterUpdateService;
            Plugins = new ObservableCollection<PluginInfoRow>();
        }

        public void ViewAttached()
        {
            var filtersNames = _filtersManager.ListAvailableFilters();
            _plugins.Clear();
            var filters = filtersNames
                .Select(f => _filtersManager.GetFilterMetaInfo(f))
                .Select(f => new PluginInfoRow(f));
            foreach (var filter in filters)
            {
                _plugins.Add(filter);
            }
            var document = new FlowDocument();
            document.FontSize = 12;
            document.Blocks.Add(new Paragraph(new Bold(new Run("Всего фильтров"))));
            document.Blocks.Add(new Paragraph(new Run(_plugins.Count.ToString())));
            AboutDocument = document;
        }

        public void PluginSelectionChanged()
        {
            var document = new FlowDocument();
            var entry = SelectedPlugin.Entry;
            document.FontSize = 12;
            document.Blocks.Add(new Paragraph(new Bold(new Run("Имя"))));
            document.Blocks.Add(new Paragraph(new Run(SelectedPlugin.Name)));
            document.Blocks.Add(new Paragraph(new Bold(new Run("Тип"))));
            document.Blocks.Add(new Paragraph(new Run(entry.GetFilterType().ToString())));
            document.Blocks.Add(new Paragraph(new Bold(new Run("Группа"))));
            document.Blocks.Add(new Paragraph(new Run(entry.GetGroupType().ToString())));
            {
                document.Blocks.Add(new Paragraph(new Bold(new Run("Конструктор"))));
                var list = new List();
                foreach (var parameter in entry.ConstructorParamaters())
                {
                    var type = new Regex("[^.]+$").Match(parameter.Type.ToString()).Value;
                    list.ListItems.Add(new ListItem(new Paragraph(new Run($"{parameter.Name}\r\n{type}\r\n({parameter.Description})"))));
                }
                document.Blocks.Add(list);
            }
            {
                document.Blocks.Add(new Paragraph(new Bold(new Run("Параметры"))));
                var list = new List();
                foreach (var parameter in entry.HotParamaters())
                {
                    var type = new Regex("[^.]+$").Match(parameter.Type.ToString()).Value;
                    list.ListItems.Add(new ListItem(new Paragraph(new Run($"{parameter.Name}\r\n{type}\r\n({parameter.Description})"))));
                }
                document.Blocks.Add(list);
            }
            AboutDocument = document;
        }

        public void OpenSourceCode()
        {
            if (SelectedPlugin == null)
            {
                return;
            }
            var url = SelectedPlugin.SourceCodeLink;
            if (!(url.StartsWith("http://") || url.StartsWith("https://")))
            {
                return;
            }
            var process = new System.Diagnostics.ProcessStartInfo(url)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(process);
        }

        public void DeletePlugin()
        {
            if (SelectedPlugin == null)
            {
                return;
            }
            var pluginPath = _filtersManager.GetFilterAssembly(SelectedPlugin.Name);
            var answer = MessageBox.Show($"Вы уверены что хотите удалить плагин '{pluginPath}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer != MessageBoxResult.Yes)
            {
                return;
            }
            _filterUpdateService.ScheduleDeletion(Path.GetFileName(pluginPath));
            MessageBox.Show("Фильтр будет удален после следующего запуска", "Выполнено", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
