using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;

namespace SP.Filters.Silent
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new Silent();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return Array.Empty<DynamicParameter>();
        }

        public string FilterDescription()
        {
            return "Генератор тишины";
        }

        public string FilterName()
        {
            return "Silent";
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
            return FilterGroupType.Input;
        }

        public Uri GetSourceCodeLink()
        {
            return new Uri("https://github.com/x64BitWorm/SoundPipe/tree/main/SoundPipe/SP.Filters.Silent");
        }

        public DynamicParameter[] HotParamaters()
        {
            return Array.Empty<DynamicParameter>();
        }
    }
}
