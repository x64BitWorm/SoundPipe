namespace SP.SDK.Primitives.Types
{
    public class DynamicStreamType : IDynamicType
    {
        public DynamicStreamType()
        { }

        public Type GetValueType()
        {
            return typeof(ISoundProvider);
        }

        public bool IsValidObject(object value)
        {
            return value is ISoundProvider;
        }

        public object FromString(string from)
        {
            return from;
        }

        public string ToString(object from)
        {
            if (!(from is string or null))
            {
                throw new ArgumentException($"'string' or null as input value required");
            }
            return from?.ToString();
        }
    }
}
