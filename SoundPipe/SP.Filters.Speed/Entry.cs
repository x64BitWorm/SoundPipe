using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.Speed
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new Speed();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("input", "Входной поток", new DynamicStreamType()),
                new DynamicParameter("speed", "Скорость (1.0 - без изменений)", new DynamicFloatType(0.1f, 4.0f), 1.0f)
            };
        }

        public string FilterDescription()
        {
            return "Фильтр скорости применяемый к входному потоку";
        }

        public string FilterName()
        {
            return "Speed";
        }

        public int FilterVersion()
        {
            return 1;
        }

        public FilterType GetFilterType()
        {
            return FilterType.Provider;
        }

        public FilterGroupType GetGroupType()
        {
            return FilterGroupType.Filter;
        }

        public Uri GetSourceCodeLink()
        {
            return new Uri("https://github.com/x64BitWorm/SoundPipe/tree/main/SoundPipe/SP.Filters.Speed");
        }

        public DynamicParameter[] HotParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("speed", "Скорость (1.0 - без изменений)", new DynamicFloatType(0.1f, 4.0f), 1.0f)
            };
        }
    }
}
