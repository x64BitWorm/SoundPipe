using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.Add
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new Add();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("input1", "Входной поток 1", new DynamicStreamType()),
                new DynamicParameter("input2", "Входной поток 2", new DynamicStreamType()),
                new DynamicParameter("mode", "Режим сложения",
                new DynamicEnumType(new string[] { "add", "div2" }), "add")
            };
        }

        public string FilterDescription()
        {
            return "Фильтр для сложения двух входных потоков";
        }

        public string FilterName()
        {
            return "Add";
        }

        public int FilterVersion()
        {
            return 2;
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
            return new Uri("https://github.com/x64BitWorm/SoundPipe/tree/main/SoundPipe/SP.Filters.Add");
        }

        public DynamicParameter[] HotParamaters()
        {
            return Array.Empty<DynamicParameter>();
        }
    }
}
