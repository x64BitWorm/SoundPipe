namespace SP.SDK.Primitives.Types
{
    public class DynamicFloatType : IDynamicType
    {
        public float Min { get; private set; }
        public float Max { get; private set; }

        public DynamicFloatType(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public Type GetValueType()
        {
            return typeof(float);
        }

        public bool IsValidObject(object value)
        {
            if (value is float)
            {
                return (float)value >= Min && (float)value <= Max;
            }
            return false;
        }

        public object FromString(string from)
        {
            if (!float.TryParse(from, out var fresult))
            {
                throw new ArgumentException($"Wrong float '{from}'");
            }
            if (fresult < Min || fresult > Max)
            {
                throw new ArgumentException($"Out of range value {fresult}");
            }
            return fresult;
        }

        public string ToString(object from)
        {
            if (from is not float)
            {
                throw new ArgumentException($"'float' as input value required");
            }
            return from.ToString();
        }
    }
}
