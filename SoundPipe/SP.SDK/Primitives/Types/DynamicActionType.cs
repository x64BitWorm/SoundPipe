namespace SP.SDK.Primitives.Types
{
    public class DynamicActionType : IDynamicType
    {
        public object Parameter { get; private set; }

        public DynamicActionType(object parameter)
        {
            Parameter = parameter;
        }

        public Type GetValueType()
        {
            return typeof(object);
        }

        public bool IsValidObject(object value)
        {
            return true;
        }

        public object FromString(string from)
        {
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
