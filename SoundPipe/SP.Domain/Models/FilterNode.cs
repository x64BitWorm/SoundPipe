namespace SP.Domain.Models
{
    public class FilterNode<T>
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public FilterDependency[] Dependencies { get; set; }
        public T MetaInfo { get; set; }
    }
}
