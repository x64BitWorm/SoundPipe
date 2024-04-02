namespace SP.SDK.Exceptions
{
    public class UnattachedLinkException: Exception
    {
        public string ElementName { get; private set; }
        public int LinkNumber { get; private set; }

        public UnattachedLinkException(string elementName, int linkNumber): 
            base($"Link {linkNumber} in {elementName} not connected")
        {
            ElementName = elementName;
            LinkNumber = linkNumber;
        }
    }
}
