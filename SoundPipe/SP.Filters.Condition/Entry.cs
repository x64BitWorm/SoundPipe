using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.Condition
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new Condition();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("condition", "Входной поток условия", new DynamicStreamType()),
                new DynamicParameter("inputIf", "Входной поток при соблюдении условия", new DynamicStreamType()),
                new DynamicParameter("inputElse", "Входной поток при несоблюдении условия", new DynamicStreamType()),
            };
        }

        public string FilterDescription()
        {
            return "Фильтр условного входного потока";
        }

        public string FilterName()
        {
            return "Condition";
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
            return FilterGroupType.Logic;
        }

        public Uri GetSourceCodeLink()
        {
            return new Uri("https://github.com/x64BitWorm/SoundPipe/tree/main/SoundPipe/SP.Filters.Condition");
        }

        public DynamicParameter[] HotParamaters()
        {
            return Array.Empty<DynamicParameter>();
        }
    }
}
