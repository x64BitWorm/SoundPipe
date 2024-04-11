using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.SinGenerator
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new SinGenerator();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("discretization", "Частота дискретизации звука", new DynamicStringType(6), "44100"),
                new DynamicParameter("frequency", "Частота волны", new DynamicIntType(20, 20000), 200),
                new DynamicParameter("volume", "Громкость волны", new DynamicFloatType(0.0f, 1.0f), 0.5f)
            };
        }

        public string FilterDescription()
        {
            return "Генератор sin волны";
        }

        public string FilterName()
        {
            return "SinGenerator";
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
            return new Uri("https://github.com/x64BitWorm/SoundPipe/tree/main/SoundPipe/SP.Filters.SinGenerator");
        }

        public DynamicParameter[] HotParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("frequency", "Частота волны", new DynamicIntType(20, 20000), 200),
                new DynamicParameter("volume", "Громкость волны", new DynamicFloatType(0.0f, 1.0f), 0.5f)
            };
        }
    }
}
