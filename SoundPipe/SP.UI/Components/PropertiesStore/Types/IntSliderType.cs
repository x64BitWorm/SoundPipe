using SP.SDK.Primitives.Types;
using SP.SDK.Primitives;

namespace SP.UI.Components.PropertiesStore.Types
{
    public class IntSliderType: PropertyType
    {
        private int _current;

        public int Min { get; set; }
        public int Max { get; set; }
        public int Current
        {
            get => _current;
            set
            {
                _current = value;
                OnPropertyChanged(nameof(_current));
            }
        }

        public override void SetDefaultValue(object value)
        {
            if (value is int)
            {
                value = (int)(double)value;
            }
            Current = (int)value;
        }

        public static IntSliderType FromParameterType(DynamicParameter parameter, DynamicIntType intType)
        {
            return new IntSliderType()
            {
                Min = intType.Min,
                Max = intType.Max
            };
        }
    }
}
