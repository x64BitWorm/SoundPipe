using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.LowPass
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new LowPass();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("input", "Входной поток", new DynamicStreamType()),
                new DynamicParameter("delay", "Коэффициент задержки (0.0 - входные данные без изменений)\nРасчитан как корень 8 степени", new DynamicFloatType(0.0f, 1.0f), 0.01f)
            };
        }

        public string FilterDescription()
        {
            return "Фильтр низких частот";
        }

        public string FilterName()
        {
            return "LowPass";
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
            return new Uri("https://github.com/x64BitWorm/SoundPipe/tree/main/SoundPipe/SP.Filters.LowPass");
        }

        public DynamicParameter[] HotParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("delay", "Коэффициент задержки (0.0 - входные данные без изменений)", new DynamicFloatType(0.0f, 1.0f), 0.01f)
            };
        }
    }
}
