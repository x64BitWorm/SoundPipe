using System;
using System.Windows;

namespace SP.UI.ViewModels
{
    public partial class MainViewModel
    {
        public void RunSheme()
        {
            if (GetShemeState() != Models.ShemeState.Constructor)
            {
                StatusBarSend.PushMessage("Схема уже запущена", Components.StatusBar.StatusMessageType.Warning, true);
                return;
            }
            try
            {
                SyncUiGraphToGraphConstructor();
                _pipeSheme = _shemeConstructor.ConstructSheme();
            }
            catch (Exception e)
            {
                StatusBarSend.PushMessage(e.ToString(), Components.StatusBar.StatusMessageType.Error, true);
                return;
            }
            StatusBarSend.PushMessage("Сейчас запущена симуляция", Components.StatusBar.StatusMessageType.Run);
            PropertyTypes = new Components.PropertiesStore.PropertyType[0];
        }

        public void StopSheme()
        {
            if (GetShemeState() != Models.ShemeState.Running)
            {
                StatusBarSend.PushMessage("Схема не запущена", Components.StatusBar.StatusMessageType.Warning, true);
                return;
            }
            try
            {
                _pipeSheme.Destroy();
                _pipeSheme = null;
                SyncGraphConstructorToUiGraph();
            }
            catch (Exception e)
            {
                StatusBarSend.PushMessage(e.ToString(), Components.StatusBar.StatusMessageType.Error, true);
                return;
            }
            StatusBarSend.PushMessage("Режим конструктора", Components.StatusBar.StatusMessageType.Default);
            PropertyTypes = new Components.PropertiesStore.PropertyType[0];
        }

        public void ShowShemeStats()
        {
            MessageBox.Show($"Всего фильтров: {_shemeConstructor.GetFilterIds().Length}", 
                "Статистика схемы", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
