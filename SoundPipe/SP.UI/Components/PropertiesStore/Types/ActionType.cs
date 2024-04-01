using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.UI.Components.PropertiesStore.Types
{
    public class ActionType: PropertyType
    {
        public string ActionTitle { get; set; }

        public override void SetDefaultValue(object value)
        {
            // nothing
        }

        public static ActionType FromParameterType(DynamicParameter parameter, DynamicActionType actionType)
        {
            return new ActionType()
            {
                ActionTitle = actionType.Parameter.ToString()
            };
        }
    }
}
