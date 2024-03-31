namespace SP.Domain.Models
{
    public class FilterNodeJson<T>
    {
        public FilterJson Filter { get; set; }
        public T Meta { get; set; }
    }
}
