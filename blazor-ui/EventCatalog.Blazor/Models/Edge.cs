namespace EventCatalog.Blazor.Models
{
    public class Edge
    {
        public string Id { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Label { get; set; }
        public bool Animated { get; set; }
        public string Type { get; set; }
        public EdgeStyle Style { get; set; }
        public Marker EndMarker { get; set; }
    }

    public class EdgeStyle
    {
        public int StrokeWidth { get; set; }
        public string Stroke { get; set; }
        public string StrokeDasharray { get; set; }
    }

    public class Marker
    {
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Color { get; set; }
    }
}
