namespace SP.UI.Components.PropertiesStore
{
    public class ChangeEventParameter
    {
        public string Id { get; set; }
        public object Value { get; set; }

        public ChangeEventParameter(string id, object value)
        {
            Id = id;
            Value = value;
        }
    }
}
