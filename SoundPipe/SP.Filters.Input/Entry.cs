using SP.SDK;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.Input
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new Input();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("fileName", "Имя входного WAV файла", new DynamicStringType(256))
            };
        }

        public string FilterDescription()
        {
            return "Фильтр представляющий входной поток из WAV файла";
        }

        public string FilterName()
        {
            return "Input";
        }

        public int FilterVersion()
        {
            return 1;
        }

        public FilterType GetFilterType()
        {
            return FilterType.Provider;
        }

        public DynamicParameter[] HotParamaters()
        {
            return Array.Empty<DynamicParameter>();
        }
    }
}
