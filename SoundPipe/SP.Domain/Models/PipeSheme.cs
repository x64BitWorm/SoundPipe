using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Domain.Models
{
    public class PipeSheme
    {
        private readonly Dictionary<string, (IFilterEntry Entry, ISoundFilter Filter)> _filters;
        
        public PipeSheme(Dictionary<string, (IFilterEntry, ISoundFilter)> filters)
        {
            _filters = filters;
        }

        public void SetVar(string name, string key, object value)
        {
            if (!_filters.ContainsKey(name))
            {
                throw new InvalidOperationException($"Sheme has no '{name}' filter");
            }
            var filter = _filters[name];
            var parameter = filter.Entry.HotParamaters().FirstOrDefault(p => p.Name == key);
            if (parameter == null)
            {
                throw new InvalidOperationException($"Filter '{name}' has no '{key}' parameter");
            }
            if(!parameter.Type.IsValidObject(value))
            {
                throw new InvalidOperationException($"Object '{value}' is not assignable to '{key}' parameter");
            }
            filter.Filter.SetHotValue(key, value);
        }

        public object GetVar(string name, string key)
        {
            if (!_filters.ContainsKey(name))
            {
                throw new InvalidOperationException($"Sheme has no '{name}' filter");
            }
            var filter = _filters[name];
            var parameter = filter.Entry.HotParamaters().FirstOrDefault(p => p.Name == key);
            if (parameter == null)
            {
                throw new InvalidOperationException($"Filter '{name}' has no '{key}' parameter");
            }
            return filter.Filter.GetHotValue(key);
        }

        public DynamicParameter[] ListHotParameters(string name)
        {
            var entry = GetFilterEntry(name);
            return entry.HotParamaters();
        }

        public IFilterEntry GetFilterEntry(string name)
        {
            return _filters[name].Item1;
        }

        public void Destroy()
        {
            foreach (var filter in _filters.OrderBy(f => f.Value.Item1.GetFilterType() == FilterType.Consumer ? 1 : 2))
            {
                filter.Value.Item2.Destroy();
            }
        }
    }
}
