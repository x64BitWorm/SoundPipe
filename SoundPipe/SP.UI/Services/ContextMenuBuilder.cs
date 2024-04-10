using SP.Domain;
using SP.SDK.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
            var groupMap = new Dictionary<FilterGroupType, ObservableCollection<MenuItem>>();
            foreach (var group in GetFilterGroupsLabels())
            {
                var subItems = new ObservableCollection<MenuItem>();
                var subItem = new MenuItem()
                {
                    Header = group.Item1,
                    ItemsSource = subItems
                };
                items.Add(subItem);
                groupMap[group.Item2] = subItems;
            }
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
                groupMap[filterInfo.GetGroupType()].Add(menuItem);
            }
            result.ItemsSource = items;
            return result;
        }

        public ContextMenu CreateNodeMenu(string nodeId, Action<string> renameNodeAction, Action<string> removeNodeAction,
            Action<string, Color> changeBorderNodeAction)
        {
            var result = new ContextMenu();
            var items = new List<MenuItem>();
            var menuItem = new MenuItem()
            {
                Header = "Переименовать",
            };
            menuItem.Click += (s, e) => renameNodeAction(nodeId);
            items.Add(menuItem);
            items.Add(new MenuItem()
            {
                Header = "Цвет кромки",
                ItemsSource = GetColorsItems((brush) => changeBorderNodeAction(nodeId, brush))
            });
            menuItem = new MenuItem()
            {
                Header = "Удалить",
            };
            menuItem.Click += (s, e) => removeNodeAction(nodeId);
            items.Add(menuItem);
            result.ItemsSource = items;
            return result;
        }

        private IEnumerable<(string, FilterGroupType)> GetFilterGroupsLabels()
        {
            yield return ("Источник", FilterGroupType.Input);
            yield return ("Потребитель", FilterGroupType.Consumer);
            yield return ("Фильтр", FilterGroupType.Filter);
            yield return ("Анализатор", FilterGroupType.Analyzer);
            yield return ("Логика", FilterGroupType.Logic);
            yield return ("Прочее", FilterGroupType.Other);
        }

        private IEnumerable<MenuItem> GetColorsItems(Action<Color> changeBorderNodeAction)
        {
            var colors = new (string Name, Color Color)[]
            {
                new("Черный", Colors.Black),
                new("Серый", Colors.Gray),
                new("Желтый", Colors.Yellow),
                new("Красный", Colors.Red),
                new("Оранжевый", Colors.Orange),
                new("Зеленый", Colors.Green),
                new("Лайм", Colors.Lime),
                new("Синий", Colors.Blue),
                new("Голубой", Colors.SkyBlue),
                new("Фиолетовый", Colors.Violet),
            };
            foreach (var color in colors)
            {
                var menuItem = new MenuItem()
                {
                    Header = color.Name,
                    Tag = color.Color,
                    Icon = new Ellipse()
                    {
                        Fill = new SolidColorBrush(color.Color)
                    }
                };
                menuItem.Click += (s, e) => changeBorderNodeAction((Color)(s as MenuItem).Tag);
                yield return menuItem;
            }
        }
    }
}
