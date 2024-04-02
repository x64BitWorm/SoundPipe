namespace SP.SDK.Exceptions
{
    public class LoadFilterException: Exception
    {
        public string FilterName { get; private set; }
        public string InnerMessage { get; private set; }

        public LoadFilterException(string filterName, string message): 
            base($"Cannot load filter '{filterName}' - {message}")
        {
            FilterName = filterName;
            InnerMessage = message;
        }
    }
}
