using SP.SDK.Primitives.Types;
using SP.SDK.Primitives;

namespace SP.UI.Components.PropertiesStore.Types
{
    public class ComboType: PropertyType
    {
        private string _selectedOption;

        public string[] Options { get; set; }
        public string SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                OnPropertyChanged(nameof(SelectedOption));
            }
        }

        public override void SetDefaultValue(object value)
        {
            if (value is string)
            {
                SelectedOption = (string)value;
            }
        }

        public static ComboType FromParameterType(DynamicParameter parameter, DynamicEnumType enumType)
        {
            return new ComboType()
            {
                Options = enumType.Options,
            };
        }
    }
}
