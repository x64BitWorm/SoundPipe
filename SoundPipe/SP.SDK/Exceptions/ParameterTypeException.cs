namespace SP.SDK.Exceptions
{
    public class ParameterTypeException: Exception
    {
        public string ElementName { get; private set; }
        public string ParameterMessage { get; private set; }

        public ParameterTypeException(string elementName, string parameterMessage): 
            base($"Exception while {elementName} element initialization - {parameterMessage}")
        {
            ElementName = elementName;
            ParameterMessage = parameterMessage;
        }
    }
}
