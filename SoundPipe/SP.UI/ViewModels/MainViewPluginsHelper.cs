using Microsoft.Extensions.DependencyInjection;
using SP.UI.Views;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SP.UI.ViewModels
{
    partial class MainViewModel
    {
        public void AddNewPlugin()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Title = "Добавление плагина";
            fileDialog.Filter = "Sound Pipe Плагин (*.dll)|*.dll";
            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var filePath = fileDialog.FileName;
            try
            {
                var fileName = Path.GetFileName(filePath);
                var entries = _filtersManager.ReadFilterEntries(filePath);
                var filters = string.Join("\r\n", entries.Select(entry => $"{entry.FilterName()} (версия {entry.FilterVersion()})"));
                var filtersDir = _settingsModel.FiltersPath;
                var newFilePath = Path.Combine(filtersDir, fileName);
                if (File.Exists(newFilePath))
                {
                    StatusBarSend.PushMessage($"Файл фильтра уже установлен -\r\n{newFilePath}", Components.StatusBar.StatusMessageType.Warning, true, "Заменить", () =>
                    {
                        _filterUpdateService.ScheduleUpdate(filePath);
                        StatusBarSend.PushMessage($"Фильтр будет заменен после перезапуска", Components.StatusBar.StatusMessageType.Success, true);
                    });
                    return;
                }
                File.Copy(filePath, newFilePath, false);
                StatusBarSend.PushMessage($"Добавленные фильтры -\r\n{filters}", Components.StatusBar.StatusMessageType.Success, true);
            }
            catch (Exception e)
            {
                StatusBarSend.PushMessage($"Невозможно добавить фильтры -\r\n{e.Message}\r\nВозможно был выбран неверный файл", Components.StatusBar.StatusMessageType.Error, true);
            }
        }

        public void ShowPluginsManager()
        {
            _serviceProvider.GetService<PluginsWindow>().ShowDialog();
        }

        public void ShowSettingsWindow()
        {
            _serviceProvider.GetService<SettingsWindow>()?.ShowDialog();
        }
    }
}
