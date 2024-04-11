namespace SP.SDK.Exceptions
{
    public class InitializeFilterException: Exception
    {
        public string FilterName { get; private set; }
        public Exception InnerException { get; private set; }
        
        public InitializeFilterException(string filterName, Exception innerException = null) :
            base($"Cannot initialize filter '{filterName}' ({innerException?.Message})")
        {
            FilterName = filterName;
            InnerException = innerException;
        }
    }
}
