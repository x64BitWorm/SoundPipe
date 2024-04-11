using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.VolumeGate
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new VolumeGate();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("input", "Входной поток", new DynamicStreamType()),
                new DynamicParameter("level", "Уровень громкости для сигнала", new DynamicFloatType(0.0f, 1.0f), 0.75f)
            };
        }

        public string FilterDescription()
        {
            return "Фильтр замера уровня громкости (сигнализатор)";
        }

        public string FilterName()
        {
            return "VolumeGate";
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
            return new Uri("https://github.com/x64BitWorm/SoundPipe/tree/main/SoundPipe/SP.Filters.VolumeGate");
        }

        public DynamicParameter[] HotParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("level", "Уровень громкости для сигнала", new DynamicFloatType(0.0f, 1.0f), 0.75f)
            };
        }
    }
}