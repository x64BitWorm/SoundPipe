namespace SP.SDK.Primitives.Types
{
    public class DynamicEnumType : IDynamicType
    {
        public string[] Options { get; private set; }

        public DynamicEnumType(string[] options)
        {
            Options = options;
        }

        public Type GetValueType()
        {
            return typeof(string);
        }

        public bool IsValidObject(object value)
        {
            return value is string && Options.Contains(value as string);
        }

        public object FromString(string from)
        {
            if (!Options.Contains(from))
            {
                throw new ArgumentException($"Unknown option '{from}' in enum");
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
