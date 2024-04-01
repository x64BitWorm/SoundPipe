using SP.SDK.Primitives.Types;
using SP.SDK.Primitives;

namespace SP.UI.Components.PropertiesStore.Types
{
    public class SliderType: PropertyType
    {
        private float _current;

        public float Min { get; set; }
        public float Max { get; set; }
        public float Current
        {
            get => _current;
            set
            {
                _current = value;
                OnPropertyChanged(nameof(Current));
            }
        }

        public override void SetDefaultValue(object value)
        {
            if (value is double)
            {
                value = (float)(double)value;
            }
            Current = (float)value;
        }

        public static SliderType FromParameterType(DynamicParameter parameter, DynamicFloatType floatType)
        {
            return new SliderType()
            {
                Min = floatType.Min,
                Max = floatType.Max
            };
        }
    }
}
