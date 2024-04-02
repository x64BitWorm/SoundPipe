using SP.UI.Models;
using SP.UI.Services;
using System.ComponentModel;

namespace SP.UI.ViewModels
{
    public class SettingsViewModel: INotifyPropertyChanged
    {
        private readonly SettingsProvider _settingsProvider;

        private SettingsModel _settingsModel;

        public SettingsModel SettingsModel
        {
            get => _settingsModel;
            set
            {
                _settingsModel = value;
                OnPropertyChanged(nameof(SettingsModel));
            }
        }

        public SettingsViewModel(SettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public void ViewLoaded()
        {
            SettingsModel = _settingsProvider.Load();
        }

        public void SaveChanges()
        {
            _settingsProvider.Save(SettingsModel);
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
