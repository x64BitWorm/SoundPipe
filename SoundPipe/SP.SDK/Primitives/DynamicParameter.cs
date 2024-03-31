namespace SP.SDK.Primitives
{
    public class DynamicParameter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IDynamicType Type { get; set; }
        public object DefaultValue { get; set; }

        public DynamicParameter(string name, string description, IDynamicType type)
        {
            Name = name;
            Description = description;
            Type = type;
            DefaultValue = null;
        }

        public DynamicParameter(string name, string description, IDynamicType type, object defaultValue)
            : this(name, description, type)
        {
            DefaultValue = defaultValue;
        }
    }
}
