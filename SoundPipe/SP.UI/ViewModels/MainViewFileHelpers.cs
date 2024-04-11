using SP.SDK.Exceptions;
using SP.UI.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace SP.UI.ViewModels
{
    public partial class MainViewModel
    {
        private bool _shemeSaved = true;
        private string _openedFileName = null;

        public bool IsShemeSaved => _shemeSaved;

        public void CreateEmptyShemeMenu()
        {
            if (!_shemeSaved)
            {
                var answer = System.Windows.MessageBox.Show("Изменения в текущей схеме будут потеряны, продолжить?",
                    "Есть несохраненные изменения", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.No)
                {
                    return;
                }
            }
            _shemeConstructor = _shemeProvider.CreateEmptySheme();
            _openedFileName = null;
            SyncGraphConstructorToUiGraph();
            StopPipeSheme();
            UpdateTitle();
        }

        public void SaveShemeToFile(bool saveAsNew)
        {
            string fileName;
            if (saveAsNew || _openedFileName == null)
            {
                var fileDialog = new SaveFileDialog();
                fileDialog.Title = "Сохранение схемы";
                fileDialog.Filter = "Sound Pipe Файлы (*.spf)|*.spf";
                fileDialog.RestoreDirectory = true;
                if (fileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                fileName = fileDialog.FileName;
            }
            else
            {
                fileName = _openedFileName;
            }
            SyncUiGraphToGraphConstructor();
            var content = _shemeManager.WriteShemeConstructor(_shemeConstructor);
            try
            {
                File.WriteAllText(fileName, content);
                StatusBarSend.PushMessage("Схема сохранена", Components.StatusBar.StatusMessageType.Success, true);
                _openedFileName = fileName;
            }
            catch
            {
                StatusBarSend.PushMessage("Не удалось сохранить схему", Components.StatusBar.StatusMessageType.Error, true);
            }
            _shemeSaved = true;
            UpdateTitle();
        }

        public void LoadShemeFromFile()
        {
            if (!_shemeSaved)
            {
                var answer = System.Windows.MessageBox.Show("Изменения в текущей схеме будут потеряны, продолжить?",
                    "Есть несохраненные изменения", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.No)
                {
                    return;
                }
            }
            var fileDialog = new OpenFileDialog();
            fileDialog.Title = "Открытие схемы";
            fileDialog.Filter = "Sound Pipe Файлы (*.spf)|*.spf";
            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var fileName = fileDialog.FileName;
            try
            {
                var content = File.ReadAllText(fileName);
                StopPipeSheme();
                _shemeConstructor = _shemeManager.ReadShemeConstructor<UINodeInfo>(content, _settingsModel.LoadShemeWithOldFilters);
                SyncGraphConstructorToUiGraph();
                StatusBarSend.PushMessage("Схема загружена", Components.StatusBar.StatusMessageType.Success, true);
                _openedFileName = fileName;
            }
            catch (LoadFilterException e)
            {
                StatusBarSend.PushMessage($"Не удалось загрузить фильтр '{e.FilterName}'\r\n{e.InnerMessage}\r\nВозможно плагин этого фильтра не установлен", 
                    Components.StatusBar.StatusMessageType.Error, true);
            }
            catch (Exception e)
            {
                StatusBarSend.PushMessage($"Не удалось открыть схему - {e.Message}", Components.StatusBar.StatusMessageType.Error, true);
            }
            UpdateTitle();
        }

        public void NotifyShemeModified()
        {
            _shemeSaved = false;
        }
    }
}
