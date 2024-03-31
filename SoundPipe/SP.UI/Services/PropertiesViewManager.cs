using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;
using SP.UI.Components.PropertiesStore;
using SP.UI.Components.PropertiesStore.Types;
using System.Collections.Generic;
using System.Linq;

namespace SP.UI.Services
{
    public class PropertiesViewManager
    {
        public IEnumerable<PropertyType> ConvertToViewTypes(IEnumerable<DynamicParameter> parameters)
        {
            return parameters.Select(ConvertToViewType);
        }

        public void ApplyDefaults(PropertyType[] types, object[] values)
        {
            if (types.Length != values.Length)
            {
                return;
            }
            for (int i = 0; i < types.Length; i++)
            {
                SetDefaultValue(ref types[i], values[i]);
            }
        }

        private PropertyType ConvertToViewType(DynamicParameter parameter)
        {
            if (parameter.Type is DynamicStringType stringType)
            {
                return FromStringType(parameter, stringType);
            }
            if (parameter.Type is DynamicFloatType floatType)
            {
                return FromFloatType(parameter, floatType);
            }
            if (parameter.Type is DynamicEnumType enumType)
            {
                return FromEnumType(parameter, enumType);
            }
            return null;
        }

        private PropertyType FromStringType(DynamicParameter parameter, DynamicStringType stringType)
        {
            return new InputType()
            {
                Id = parameter.Name,
                Label = parameter.Name
            };
        }

        private PropertyType FromFloatType(DynamicParameter parameter, DynamicFloatType floatType)
        {
            return new SliderType()
            {
                Id = parameter.Name,
                Label = parameter.Name,
                Min = floatType.Min,
                Max = floatType.Max
            };
        }

        private PropertyType FromEnumType(DynamicParameter parameter, DynamicEnumType enumType)
        {
            return new ComboType()
            {
                Id = parameter.Name,
                Label = parameter.Name,
                Options = enumType.Options,
            };
        }

        private void SetDefaultValue(ref PropertyType type, object value)
        {
            if (type is InputType inputType)
            {
                inputType.DefaultValue = value?.ToString() ?? string.Empty;
            }
            else if (type is SliderType sliderType)
            {
                if (value is double)
                {
                    value = (float)(double)value;
                }
                sliderType.Current = (float)value;
            }
            else if (type is ComboType comboType)
            {
                if (value is string)
                {
                    comboType.SelectedOption = (string)value;
                }
            }
        }
    }
}
