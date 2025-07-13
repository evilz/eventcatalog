namespace EventCatalog.Blazor.Models
{
    public class Node<T>
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public Position Position { get; set; }
        public T Data { get; set; }
    }

    public class Position
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
