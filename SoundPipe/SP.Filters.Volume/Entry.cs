using SP.SDK;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.Volume
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new Volume();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("input", "Входной поток", new DynamicStreamType()),
                new DynamicParameter("level", "Уровень громкости (1.0 - без изменений)", new DynamicFloatType(-3.0f, +3.0f), 1.0f)
            };
        }

        public string FilterDescription()
        {
            return "Фильтр громкости применяемый к входному потоку";
        }

        public string FilterName()
        {
            return "Volume";
        }

        public int FilterVersion()
        {
            return 1;
        }

        public FilterType GetFilterType()
        {
            return FilterType.Provider;
        }

        public DynamicParameter[] ReadableHotParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("level", "Уровень громкости (1.0 - без изменений)", new DynamicFloatType(-3.0f, +3.0f))
            };
        }

        public DynamicParameter[] WritableHotParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("level", "Уровень громкости (1.0 - без изменений)", new DynamicFloatType(-3.0f, +3.0f))
            };
        }
    }
}
