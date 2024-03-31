using SP.SDK;

namespace SP.Domain.Models
{
    public class FilterTemplate<T>
    {
        public string Id { get; set; }
        public IFilterEntry Entry { get; set; }
        public object[] Parameters { get; set; }
        public T MetaInfo { get; set; }

    }
}
