using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.SaveToFile
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new SaveToFile();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("input", "Входной поток", new DynamicStreamType()),
                new DynamicParameter("fileName", "Имя файла для записи", new DynamicStringType(200)),
                new DynamicParameter("duration", "Длительность сохранения в секундах", new DynamicIntType(1, 24 * 60 * 60), 60)
            };
        }

        public string FilterDescription()
        {
            return "Сохранение потока в WAV файл (дискретизация - 44100)";
        }

        public string FilterName()
        {
            return "SaveToFile";
        }

        public int FilterVersion()
        {
            return 1;
        }

        public FilterType GetFilterType()
        {
            return FilterType.Consumer;
        }

        public FilterGroupType GetGroupType()
        {
            return FilterGroupType.Consumer;
        }

        public Uri GetSourceCodeLink()
        {
            return new Uri("https://github.com/x64BitWorm/SoundPipe/tree/main/SoundPipe/SP.Filters.SaveToFile");
        }

        public DynamicParameter[] HotParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("save", "Действие сохранения", new DynamicActionType("Сохранить"),
                HotParameterType.WriteOnly)
            };
        }
    }
}
