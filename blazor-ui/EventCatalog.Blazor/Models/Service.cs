using System.Collections.Generic;

namespace EventCatalog.Blazor.Models
{
    public class Service
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Summary { get; set; }
        public List<MessageSummary> Receives { get; set; }
        public List<MessageSummary> Sends { get; set; }
        public List<object> Owners { get; set; }
        public List<object> Tags { get; set; }
        public object ExternalLinks { get; set; }
        public object Badge { get; set; }
    }

    public class ServiceSummary
    {
        public string Id { get; set; }
        public string Version { get; set; }
    }
}
