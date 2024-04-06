using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.Channel
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new Channel();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("input", "Входной поток", new DynamicStreamType()),
                new DynamicParameter("mode", "Режим выходного потока", 
                new DynamicEnumType(new string[] { "normal", "left", "right", "swap", "between" }), "normal")
            };
        }

        public string FilterDescription()
        {
            return "Фильтр левого и правого канала";
        }

        public string FilterName()
        {
            return "Channel";
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
            return new Uri("https://github.com/x64BitWorm/SoundPipe/tree/main/SoundPipe/SP.Filters.Channel");
        }

        public DynamicParameter[] HotParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("mode", "Режим выходного потока",
                new DynamicEnumType(new string[] { "normal", "left", "right", "swap", "between" }), "normal")
            };
        }
    }
}
