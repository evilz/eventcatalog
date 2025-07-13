using System.Collections.Generic;

namespace EventCatalog.Blazor.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Summary { get; set; }
        public List<ServiceSummary> Producers { get; set; }
        public List<ServiceSummary> Consumers { get; set; }
        public List<ChannelSummary> Channels { get; set; }
        public List<object> Owners { get; set; }
        public List<object> Tags { get; set; }
        public object ExternalLinks { get; set; }
        public object Badge { get; set; }
    }

    public class MessageSummary
    {
        public string Id { get; set; }
        public string Version { get; set; }
    }

    public class ChannelSummary
    {
        public string Id { get; set; }
        public string Version { get; set; }
    }
}
