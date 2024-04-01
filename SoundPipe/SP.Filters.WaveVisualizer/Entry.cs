using SP.SDK;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.WaveVisualizer
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new WaveVisualizer();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("input", "Входной поток", new DynamicStreamType()),
            };
        }

        public string FilterDescription()
        {
            return "Визуализатор звуковой волны";
        }

        public string FilterName()
        {
            return "WaveVisualizer";
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
                new DynamicParameter("image", "Изображение содержащее звуковую волну", new DynamicImageType(512, 512),
                HotParameterType.ReadOnly)
            };
        }
    }
}
