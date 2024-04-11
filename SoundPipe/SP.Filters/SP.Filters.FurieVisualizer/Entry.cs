using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.FurieVisualizer
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new FurieVisualizer();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("input", "Входной поток", new DynamicStreamType()),
                new DynamicParameter("zoom", "Степень увеличения шкалы по Y", new DynamicIntType(1, 128), 1)
            };
        }

        public string FilterDescription()
        {
            return "Частотный визуализатор звуковой волны";
        }

        public string FilterName()
        {
            return "FurieVisualizer";
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
            return FilterGroupType.Analyzer;
        }

        public Uri GetSourceCodeLink()
        {
            return new Uri("https://github.com/x64BitWorm/SoundPipe/tree/main/SoundPipe/SP.Filters.FurieVisualizer");
        }

        public DynamicParameter[] HotParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("image", "Изображение содержащее частотный анализ", new DynamicImageType(512, 512),
                HotParameterType.ReadOnly)
            };
        }
    }
}
