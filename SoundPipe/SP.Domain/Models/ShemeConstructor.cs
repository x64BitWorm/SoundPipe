using SP.SDK;
using SP.SDK.Exceptions;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Domain.Models
{
    public class ShemeConstructor<T>
    {
        private readonly FiltersManager _filtersManager;
        private readonly Dictionary<string, FilterTemplate<T>> _filters;

        public ShemeConstructor(FiltersManager filterManager)
        {
            _filtersManager = filterManager;
            _filters = new Dictionary<string, FilterTemplate<T>>();
        }

        public FilterTemplate<T> this[string id]
        {
            get => _filters[id];
        }

        public void AddFilter(string id, string filterName, T metaInfo)
        {
            var filterEntry = _filtersManager.GetFilterMetaInfo(filterName);
            var parametersProto = filterEntry.ConstructorParamaters();
            var parameters = parametersProto.Select(p => p.DefaultValue).ToArray();
            var template = new FilterTemplate<T>()
            {
                Id = id,
                Entry = filterEntry,
                MetaInfo = metaInfo,
                Parameters = parameters
            };
            _filters.Add(id, template);
        }

        public void RemoveFilter(string id)
        {
            foreach (var filter in _filters.Values)
            {
                var linkIndexes = GetLinkIndexes(filter.Id);
                foreach (var linkIndex in linkIndexes)
                {
                    if (filter.Parameters[linkIndex] is string name && name == id)
                    {
                        filter.Parameters[linkIndex] = null;
                    }
                }
            }
            _filters.Remove(id);
        }

        public DynamicParameter[] GetFilterParameters(string id)
        {
            return _filters[id].Entry.ConstructorParamaters()
                .Where(p => p.Type is not DynamicStreamType)
                .ToArray();
        }

        public void SetParameterValue(string id, string parameterName, object value)
        {
            var index = GetParameterIndex(id, parameterName);
            _filters[id].Parameters[index] = value;
        }

        public object GetParameterValue(string id, string parameterName)
        {
            var index = GetParameterIndex(id, parameterName);
            return _filters[id].Parameters[index];
        }

        public FilterNode<T>[] GetLinks()
        {
            var result = _filters.Values.Select(f => new FilterNode<T>
            {
                Id = f.Id,
                Type = f.Entry.FilterName(),
                MetaInfo = f.MetaInfo,
            }).ToArray();
            foreach (var filter in result)
            {
                var filterEntry = _filters[filter.Id];
                var constructorParameters = filterEntry.Entry.ConstructorParamaters();
                var parameters = GetLinkIndexes(filter.Id)
                    .Select(p => new FilterDependency()
                    {
                        Id = (string)filterEntry.Parameters[p],
                        Description = constructorParameters[p].Description
                    })
                    .ToArray();
                filter.Dependencies = parameters;
            }
            return result;
        }

        public void SetLinks(FilterNode<T>[] nodes)
        {
            foreach (var node in nodes)
            {
                var filter = _filters[node.Id];
                var linkIndexes = GetLinkIndexes(node.Id);
                filter.MetaInfo = node.MetaInfo;
                node.Dependencies.Zip(linkIndexes, (d, i) => filter.Parameters[i] = d.Id).ToArray();
            }
        }

        public string[] GetFilterIds()
        {
            return _filters.Keys.ToArray();
        }

        public PipeSheme ConstructSheme()
        {
            var result = new Dictionary<string, (IFilterEntry, ISoundFilter)>();
            foreach (var filterProto in _filters)
            {
                var template = filterProto.Value;
                var filter = template.Entry.ConstructFilter();
                result.Add(template.Id, (template.Entry, filter));
            }
            foreach (var filterProto in _filters)
            {
                var template = filterProto.Value;
                var (filterInfo, filter) = result[filterProto.Key];
                var parameters = template.Parameters.ToArray();
                var linkIndexes = GetLinkIndexes(template.Id);
                foreach (var index in linkIndexes)
                {
                    if (parameters[index] is not string)
                    {
                        throw new UnattachedLinkException(filterProto.Key, index);
                    }
                    parameters[index] = result[(string)parameters[index]].Item2;
                }
                try
                {
                    filter.Initialize(parameters);
                }
                catch (Exception e)
                {
                    throw new ParameterTypeException(filterProto.Value.Id, e.Message);
                }
            }
            return new PipeSheme(result);
        }

        private int[] GetLinkIndexes(string id)
        {
            return _filters[id].Entry.ConstructorParamaters()
                    .Select((p, i) => (p, i))
                    .Where(p => p.p.Type is DynamicStreamType)
                    .Select(p => p.i)
                    .ToArray();
        }

        private int GetParameterIndex(string id, string parameterName)
        {
            var parameters = _filters[id].Entry.ConstructorParamaters();
            for (var result = 0; result < parameters.Length; result++)
            {
                if (parameters[result].Name == parameterName)
                {
                    return result;
                }
            }
            return -1;
        }
    }
}
