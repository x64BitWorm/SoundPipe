using SP.SDK;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.Hub
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new Hub();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("input", "Входной поток для раздвоения", new DynamicStreamType())
            };
        }

        public string FilterDescription()
        {
            return "Фильтр для раздвоения входного потока";
        }

        public string FilterName()
        {
            return "Hub";
        }

        public int FilterVersion()
        {
            return 1;
        }

        public FilterType GetFilterType()
        {
            return FilterType.SequentialProvider;
        }

        public DynamicParameter[] HotParamaters()
        {
            return Array.Empty<DynamicParameter>();
        }
    }
}
