using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.ButtonGate
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new ButtonGate();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return Array.Empty<DynamicParameter>();
        }

        public string FilterDescription()
        {
            return "Сигнализатор о нажатии кнопки";
        }

        public string FilterName()
        {
            return "ButtonGate";
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
            return new Uri("https://github.com/x64BitWorm/SoundPipe/tree/main/SoundPipe/SP.Filters.ButtonGate");
        }

        public DynamicParameter[] HotParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("peek", "Нажатие кнопки", new DynamicActionType("Сигнал"),
                HotParameterType.WriteOnly)
            };
        }
    }
}
