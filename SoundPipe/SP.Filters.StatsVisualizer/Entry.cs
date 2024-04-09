using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.StatsVisualizer
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new StatsVisualizer();
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
            return "Статистика входного потока";
        }

        public string FilterName()
        {
            return "StatsVisualizer";
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
            return new Uri("https://github.com/x64BitWorm/SoundPipe/tree/main/SoundPipe/SP.Filters.StatsVisualizer");
        }

        public DynamicParameter[] HotParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("totalSamples", "Количество пройденных семплов", new DynamicStringType(25),
                HotParameterType.ReadOnly),
                new DynamicParameter("samplesSpeed", "Скорость прохождения семплов", new DynamicStringType(25),
                HotParameterType.ReadOnly),
                new DynamicParameter("requestsPerSecond", "Количество запросов в секунду", new DynamicStringType(25),
                HotParameterType.ReadOnly),
            };
        }
    }
}
