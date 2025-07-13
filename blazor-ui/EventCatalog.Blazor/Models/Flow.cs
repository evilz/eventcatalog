using System.Collections.Generic;

namespace EventCatalog.Blazor.Models
{
    public class Flow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Summary { get; set; }
        public List<Step> Steps { get; set; }
    }

    public class Step
    {
        public string Id { get; set; }
        public ServiceSummary Service { get; set; }
        public FlowSummary Flow { get; set; }
        public MessageSummary Message { get; set; }
        public object Actor { get; set; }
        public object Custom { get; set; }
        public object ExternalSystem { get; set; }
        public List<NextStep> NextSteps { get; set; }
        public NextStep NextStep { get; set; }
    }

    public class FlowSummary
    {
        public string Id { get; set; }
        public string Version { get; set; }
    }

    public class NextStep
    {
        public string Id { get; set; }
        public string Label { get; set; }
    }
}
