namespace SP.Domain.Models
{
    public class FilterDependency
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public FilterDependency(string id)
        {
            Id = id;
        }

        public FilterDependency() { }
    }
}
