namespace SP.SDK.Primitives.Types
{
    public class DynamicStringType : IDynamicType
    {
        public int MaxLength { get; private set; }

        public DynamicStringType(int maxLength)
        {
            MaxLength = maxLength;
        }

        public Type GetValueType()
        {
            return typeof(string);
        }

        public bool IsValidObject(object value)
        {
            return (value is string) && (value as string).Length <= MaxLength;
        }

        public object FromString(string from)
        {
            if (from.Length > MaxLength)
            {
                throw new ArgumentException($"String '{from}' is too long");
            }
            return from;
        }

        public string ToString(object from)
        {
            if (from is not string)
            {
                throw new ArgumentException($"'string' as input value required");
            }
            return from.ToString();
        }
    }
}
