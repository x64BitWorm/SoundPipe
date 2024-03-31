using SP.SDK.Primitives;

namespace SP.SDK
{
    public interface IFilterEntry
    {
        public string FilterName();
        public string FilterDescription();
        public int FilterVersion();

        public FilterType GetFilterType();
        public DynamicParameter[] ConstructorParamaters();
        public DynamicParameter[] WritableHotParamaters();
        public DynamicParameter[] ReadableHotParamaters();

        public ISoundFilter ConstructFilter();
    }
}
