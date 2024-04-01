using SP.SDK;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.VolumeLevel
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new VolumeLevel();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("input", "Входной поток", new DynamicStreamType())
            };
        }

        public string FilterDescription()
        {
            return "Фильтр замера уровня громкости от 0.0 до 1.0";
        }

        public string FilterName()
        {
            return "VolumeLevel";
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
            return new DynamicParameter[]
            {
                new DynamicParameter("level", "Уровень громкости", new DynamicFloatType(0.0f, 1.0f), 
                HotParameterType.ReadOnly)
            };
        }
    }
}