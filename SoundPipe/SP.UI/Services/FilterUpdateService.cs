using System.IO;

namespace SP.UI.Services
{
    public class FilterUpdateService
    {
        private readonly string _filtersPath;
        private string _filtersUpdatePath;

        public FilterUpdateService(SettingsProvider settingsProvider)
        {
            _filtersPath = settingsProvider.Load().FiltersPath;
            _filtersUpdatePath = Path.Combine(_filtersPath, "Updates");
            if (!Directory.Exists(_filtersUpdatePath))
            {
                Directory.CreateDirectory(_filtersUpdatePath);
            }
        }

        public void UpdateFilters()
        {
            var updates = Directory.EnumerateFiles(_filtersUpdatePath, "*.dll");
            foreach (var update in updates)
            {
                var fileName = Path.GetFileName(update);
                var newFileName = Path.Combine(_filtersPath, fileName);
                try
                {
                    File.Copy(update, newFileName, true);
                    File.Delete(update);
                }
                catch
                { }
            }
            var deletions = Directory.EnumerateFiles(_filtersUpdatePath, "*.delete");
            foreach (var deletion in deletions)
            {
                try
                {
                    var fileName = Path.GetFileName(deletion);
                    var originalPath = Path.Combine(_filtersPath, fileName.Substring(0, fileName.Length - 7));
                    File.Delete(originalPath);
                    File.Delete(deletion);
                }
                catch
                { }
            }
        }

        public void ScheduleUpdate(string pluginName)
        {
            var fileName = Path.GetFileName(pluginName);
            var newFileName = Path.Combine(_filtersUpdatePath, fileName);
            File.Copy(pluginName, newFileName, true);
        }

        public void ScheduleDeletion(string pluginName)
        {
            var deletionFile = Path.Combine(_filtersUpdatePath, pluginName + ".delete");
            File.Create(deletionFile);
        }
    }
}
