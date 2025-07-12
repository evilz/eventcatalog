namespace EventCatalog.Core
{
    public class Event
    {
        public string? Name { get; set; }
        public string? Version { get; set; }
        public string? Summary { get; set; }
        public string[]? Producers { get; set; }
        public string[]? Consumers { get; set; }
        public string[]? Owners { get; set; }
        public string? ExternalLinks { get; set; }
        public string[]? Tags { get; set; }
    }
}
