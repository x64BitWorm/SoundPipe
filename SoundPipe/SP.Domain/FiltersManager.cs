using SP.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SP.Domain
{
    public class FiltersManager
    {
        private readonly Logger _logger;
        private readonly Dictionary<string, IFilterEntry> _filterEntries;

        public FiltersManager(Logger logger)
        {
            _logger = logger;
            _filterEntries = new Dictionary<string, IFilterEntry>();
        }

        public void LoadFilters(string filtersPath)
        {
            var filtersPaths = Directory.EnumerateFiles(filtersPath, "*.dll");
            foreach (var filterPath in filtersPaths)
            {
                try
                {
                    Assembly myDll = Assembly.LoadFrom(filterPath);
                    foreach(var type in myDll.DefinedTypes.Where(t => t.IsAssignableTo(typeof(IFilterEntry))))
                    {
                        var constructor = type.GetConstructors().First(c => c.GetParameters().Length == 0);
                        var filter = constructor.Invoke(Array.Empty<object>()) as IFilterEntry;
                        _filterEntries.Add(filter.FilterName(), filter);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Unable to load filter '{filterPath}'");
                }
            }
        }

        public ISoundFilter BuildFilter(string name)
        {
            return _filterEntries[name].ConstructFilter();
        }

        public IFilterEntry GetFilterMetaInfo(string name)
        {
            return _filterEntries[name];
        }

        public IEnumerable<string> ListAvailableFilters()
        {
            return _filterEntries.Keys;
        }
    }
}
