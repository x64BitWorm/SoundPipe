using SP.SDK;

namespace SP.UI.Models
{
    public class PluginInfoRow
    {
        public string Name { get; private set; }
        public int Version { get; private set; }
        public string Description { get; private set; }
        public string SourceCodeLink { get; private set; }
        public IFilterEntry Entry { get; private set; }

        public PluginInfoRow(IFilterEntry entry)
        {
            Entry = entry;
            Name = entry.FilterName();
            Version = entry.FilterVersion();
            Description = entry.FilterDescription();
            SourceCodeLink = entry.GetSourceCodeLink().AbsoluteUri;
        }
    }
}
