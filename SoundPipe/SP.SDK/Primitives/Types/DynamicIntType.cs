namespace SP.SDK.Primitives.Types
{
    public class DynamicIntType : IDynamicType
    {
        public int Min { get; private set; }
        public int Max { get; private set; }

        public DynamicIntType(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public Type GetValueType()
        {
            return typeof(int);
        }

        public bool IsValidObject(object value)
        {
            if (value is int)
            {
                return (int)value >= Min && (int)value <= Max;
            }
            return false;
        }

        public object FromString(string from)
        {
            if (!int.TryParse(from, out var iresult))
            {
                throw new ArgumentException($"Wrong int '{from}'");
            }
            if (iresult < Min || iresult > Max)
            {
                throw new ArgumentException($"Out of range value {iresult}");
            }
            return iresult;
        }

        public string ToString(object from)
        {
            if (from is not int)
            {
                throw new ArgumentException($"'int' as input value required");
            }
            return from.ToString();
        }
    }
}
