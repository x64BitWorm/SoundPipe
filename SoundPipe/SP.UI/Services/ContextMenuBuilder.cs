using SP.Domain;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SP.UI.Services
{
    public class ContextMenuBuilder
    {
        private readonly FiltersManager _filtersManager;

        public ContextMenuBuilder(FiltersManager filtersManager)
        {
            _filtersManager = filtersManager;
        }

        public ContextMenu CreateDefaultMenu(Action<string> addNewNodeAction)
        {
            var result = new ContextMenu();
            var items = new List<MenuItem>();
            var filters = _filtersManager.ListAvailableFilters();
            foreach (var filter in filters)
            {
                var filterInfo = _filtersManager.GetFilterMetaInfo(filter);
                var menuItem = new MenuItem()
                {
                    Header = filterInfo.FilterName(),
                    Tag = filterInfo.FilterName(),
                    ToolTip = $"Версия: {filterInfo.FilterVersion()} {(filterInfo.GetFilterType() == SDK.Primitives.FilterType.Consumer ? "(Потребитель)" : "")}\r\n\r\n{filterInfo.FilterDescription()}"
                };
                menuItem.Click += (s, e) => addNewNodeAction((e.Source as MenuItem).Tag as string);
                items.Add(menuItem);
            }
            result.ItemsSource = items;
            return result;
        }

        public ContextMenu CreateNodeMenu(string nodeId, Action<string> renameNodeAction, Action<string> removeNodeAction)
        {
            var result = new ContextMenu();
            var items = new List<MenuItem>();
            var menuItem = new MenuItem()
            {
                Header = "Переименовать",
            };
            menuItem.Click += (s, e) => renameNodeAction(nodeId);
            items.Add(menuItem);
            menuItem = new MenuItem()
            {
                Header = "Удалить",
            };
            menuItem.Click += (s, e) => removeNodeAction(nodeId);
            items.Add(menuItem);
            result.ItemsSource = items;
            return result;
        }
    }
}
