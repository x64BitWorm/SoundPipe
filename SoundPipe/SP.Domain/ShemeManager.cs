using Newtonsoft.Json;
using SP.Domain.Models;
using SP.SDK;
using SP.SDK.Exceptions;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Domain
{
    public class ShemeManager
    {
        private readonly FiltersManager _filtersManager;

        public ShemeManager(FiltersManager filtersManager)
        {
            _filtersManager = filtersManager;
        }

        public PipeSheme ConstructSheme(string shemeJson)
        {
            var result = new Dictionary<string, (IFilterEntry, ISoundFilter)>();
            var filters = JsonConvert.DeserializeObject<FilterJson[]>(shemeJson);
            foreach(var filterObject in filters)
            {
                var filterInfo = _filtersManager.GetFilterMetaInfo(filterObject.Type);
                var filter = _filtersManager.BuildFilter(filterObject.Type);
                result.Add(filterObject.Id, (filterInfo, filter));
            }
            foreach (var filterObject in filters)
            {
                var (filterInfo, filter) = result[filterObject.Id];
                var constructorParams = filterInfo.ConstructorParamaters();
                if(constructorParams.Length != filterObject.Parameters.Length)
                {
                    throw new InvalidOperationException($"Wrong parameters length for '{filterObject.Type}' filter");
                }
                var parameters = constructorParams.Zip(filterObject.Parameters, (cp, fp) => ParseParameter(result, cp.Type, fp));
                filter.Initialize(parameters.ToArray());
            }
            return new PipeSheme(result);
        }

        public ShemeConstructor<T> ReadShemeConstructor<T>(string shemeJson, bool allowOldVersion = false)
        {
            var result = new ShemeConstructor<T>(_filtersManager);
            var filters = JsonConvert.DeserializeObject<FilterNodeJson<T>[]>(shemeJson);
            foreach (var filter in filters)
            {
                try
                {
                    result.AddFilter(filter.Filter.Id, filter.Filter.Type, filter.Meta);
                }
                catch (Exception e)
                {
                    throw new LoadFilterException(filter.Filter.Type, e.Message);
                }
            }
            foreach (var filterId in result.GetFilterIds())
            {
                var filter = result[filterId];
                var parametersInfo = filter.Entry.ConstructorParamaters();
                var parametersValues = filters.First(f => f.Filter.Id == filterId).Filter.Parameters;
                try
                {
                    if (parametersInfo.Length != parametersValues.Length)
                    {
                        throw new InitializeFilterException($"Wrong parameters length for '{filterId}' filter");
                    }
                    try
                    {
                        for (int i = 0; i < parametersInfo.Length; i++)
                        {
                            filter.Parameters[i] = parametersInfo[i].Type.FromString(parametersValues[i]);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new InitializeFilterException($"Cannot initialize parameter in '{filterId}'", e);
                    }
                }
                catch (InitializeFilterException e)
                {
                    if (allowOldVersion)
                    {
                        for (int i = 0; i < parametersInfo.Length; i++)
                        {
                            filter.Parameters[i] = parametersInfo[i].DefaultValue;
                        }
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
            return result;
        }

        public string WriteShemeConstructor<T>(ShemeConstructor<T> shemeConstructor)
        {
            var result = shemeConstructor.GetFilterIds()
                .Select(id => shemeConstructor[id])
                .Select(f => new FilterNodeJson<T>()
                {
                    Filter = new FilterJson()
                    {
                        Id = f.Id,
                        Parameters = f.Entry.ConstructorParamaters()
                        .Zip(f.Parameters)
                        .Select(p => p.First.Type.ToString(p.Second))
                        .ToArray(),
                        Type = f.Entry.FilterName()
                    },
                    Meta = f.MetaInfo
                }).ToArray();
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        private static object ParseParameter(Dictionary<string, (IFilterEntry, ISoundFilter)> filters, IDynamicType dynamicType, string value)
        {
            object result;
            if (dynamicType is DynamicEnumType dynamicEnumType)
            {
                result = dynamicEnumType.FromString(value);
            }
            else if (dynamicType is DynamicFloatType dynamicFloatType)
            {
                result = dynamicFloatType.FromString(value);
            }
            else if (dynamicType is DynamicIntType dynamicIntType)
            {
                result = dynamicIntType.FromString(value);
            }
            else if (dynamicType is DynamicStreamType dynamicStreamType)
            {
                if (!filters.ContainsKey(value))
                {
                    throw new ArgumentException($"Unknown filter '{value}'");
                }
                result = filters[value].Item2;
            }
            else if (dynamicType is DynamicStringType dynamicStringType)
            {
                result = dynamicStringType.FromString(value);
            }
            else
            {
                throw new ArgumentException($"Unknown type '{dynamicType.GetType().Name}'");
            }
            if (!result.GetType().IsAssignableTo(dynamicType.GetValueType()))
            {
                throw new ArgumentException($"Wrong parameter '{value}' ('{dynamicType.GetValueType().FullName}' expected but '{result.GetType().FullName}' found)");
            }
            return result;
        }
    }
}
