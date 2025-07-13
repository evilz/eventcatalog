namespace EventCatalog.Blazor.Models
{
    public class Collection<T>
    {
        public string collection { get; set; }
        public T data { get; set; }
    }
}
