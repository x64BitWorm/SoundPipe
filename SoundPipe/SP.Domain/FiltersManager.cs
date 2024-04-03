using SP.SDK;
using System.Reflection;

namespace SP.Domain
{
    public class FiltersManager
    {
        private readonly Logger _logger;
        private readonly Dictionary<string, IFilterEntry> _filterEntries;
        private readonly Dictionary<string, string> _filtersAssemblies;

        public FiltersManager(Logger logger)
        {
            _logger = logger;
            _filterEntries = new Dictionary<string, IFilterEntry>();
            _filtersAssemblies = new Dictionary<string, string>();
        }

        public void LoadFilters(string filtersPath)
        {
            var filtersPaths = Directory.EnumerateFiles(filtersPath, "*.dll");
            foreach (var filterPath in filtersPaths)
            {
                try
                {
                    var filters = ReadFilterEntries(filterPath);
                    foreach (var filter in filters)
                    {
                        var filterName = filter.FilterName();
                        _filterEntries.Add(filterName, filter);
                        _filtersAssemblies.Add(filterName, filterPath);
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

        public string GetFilterAssembly(string filterName)
        {
            return _filtersAssemblies[filterName];
        }

        public IEnumerable<string> ListAvailableFilters()
        {
            return _filterEntries.Keys;
        }

        public IEnumerable<IFilterEntry> ReadFilterEntries(string filterPath)
        {
            Assembly myDll = Assembly.LoadFrom(filterPath);
            foreach (var type in myDll.DefinedTypes.Where(t => t.IsAssignableTo(typeof(IFilterEntry))))
            {
                var constructor = type.GetConstructors().First(c => c.GetParameters().Length == 0);
                yield return constructor.Invoke(Array.Empty<object>()) as IFilterEntry;
            }
        }
    }
}
