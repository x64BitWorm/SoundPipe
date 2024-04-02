using Newtonsoft.Json;
using SP.UI.Models;
using System;
using System.IO;

namespace SP.UI.Services
{
    public class SettingsProvider
    {
        private readonly string _appFolder;
        private readonly string _settingsPath;

        public SettingsProvider()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _appFolder = Path.Combine(appDataPath, "SoundPipe");
            _settingsPath = Path.Combine(_appFolder, "settings.json");
            if (!Directory.Exists(_appFolder))
            {
                Directory.CreateDirectory(_appFolder);
            }
            if (!File.Exists(_settingsPath))
            {
                var defaults = GetDefault();
                Save(defaults);
                if (!Directory.Exists(defaults.FiltersPath))
                {
                    Directory.CreateDirectory(defaults.FiltersPath);
                }
            }
            else
            {
                var settings = Load();
                if (!Directory.Exists(settings.FiltersPath))
                {
                    settings.FiltersPath = GetDefault().FiltersPath;
                    Save(settings);
                    if (!Directory.Exists(settings.FiltersPath))
                    {
                        Directory.CreateDirectory(settings.FiltersPath);
                    }
                }
            }
        }

        public SettingsModel Load()
        {
            var content = File.ReadAllText(_settingsPath);
            return JsonConvert.DeserializeObject<SettingsModel>(content);
        }

        public void Save(SettingsModel settings)
        {
            var content = JsonConvert.SerializeObject(settings);
            File.WriteAllText(_settingsPath, content);
        }

        public SettingsModel GetDefault()
        {
            return new SettingsModel()
            {
                FiltersPath = Path.Combine(_appFolder, "Filters"),
                ParameterRefreshRate = 100
            };
        }
    }
}
