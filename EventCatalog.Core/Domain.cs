namespace EventCatalog.Core
{
    public class Domain
    {
        public string? Name { get; set; }
        public string? Summary { get; set; }
        public string[]? Owners { get; set; }
        public string[]? Tags { get; set; }
        public string? ExternalLinks { get; set; }
    }
}
