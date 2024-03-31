namespace SP.SDK.Primitives
{
    public interface IDynamicType
    {
        public Type GetValueType();
        public bool IsValidObject(object value);
        public object FromString(string from);
        public string ToString(object from);
    }
}
