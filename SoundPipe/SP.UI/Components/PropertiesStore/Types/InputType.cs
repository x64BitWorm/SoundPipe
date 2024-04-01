using SP.SDK.Primitives.Types;
using SP.SDK.Primitives;

namespace SP.UI.Components.PropertiesStore.Types
{
    public class InputType: PropertyType
    {
        private string _defaultValue;

        public string DefaultValue
        {
            get => _defaultValue;
            set
            {
                _defaultValue = value;
                OnPropertyChanged(nameof(DefaultValue));
            }
        }

        public override void SetDefaultValue(object value)
        {
            DefaultValue = value?.ToString() ?? string.Empty;
        }

        public static InputType FromParameterType(DynamicParameter parameter, DynamicStringType floatType)
        {
            return new InputType();
        }
    }
}
