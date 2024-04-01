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
                types[i].SetDefaultValue(values[i]);
            }
        }

        private PropertyType ConvertToViewType(DynamicParameter parameter)
        {
            PropertyType result;
            if (parameter.Type is DynamicStringType stringType)
            {
                result = InputType.FromParameterType(parameter, stringType);
            }
            else if (parameter.Type is DynamicFloatType floatType)
            {
                result = SliderType.FromParameterType(parameter, floatType);
            }
            else if (parameter.Type is DynamicIntType intType)
            {
                result = IntSliderType.FromParameterType(parameter, intType);
            }
            else if (parameter.Type is DynamicEnumType enumType)
            {
                result = ComboType.FromParameterType(parameter, enumType);
            }
            else if (parameter.Type is DynamicActionType actionType)
            {
                result = ActionType.FromParameterType(parameter, actionType);
            }
            else if (parameter.Type is DynamicImageType imageType)
            {
                result = ImageType.FromParameterType(parameter, imageType);
            }
            else
            {
                return null;
            }
            result.Id = parameter.Name;
            result.Label = parameter.Name;
            result.InteractionType = (PropertyInteractionType)(int)parameter.ParameterType;
            return result;
        }
    }
}
