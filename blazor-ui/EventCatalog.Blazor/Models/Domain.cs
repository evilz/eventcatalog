using System.Collections.Generic;

namespace EventCatalog.Blazor.Models
{
    public class Domain
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Summary { get; set; }
        public List<ServiceSummary> Services { get; set; }
        public List<DomainSummary> Domains { get; set; }
        public List<object> Owners { get; set; }
        public List<object> Tags { get; set; }
        public object ExternalLinks { get; set; }
        public object Badge { get; set; }
        public List<Entity> Entities { get; set; }
    }

    public class DomainSummary
    {
        public string Id { get; set; }
        public string Version { get; set; }
    }

    public class Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Summary { get; set; }
        public List<Property> Properties { get; set; }
        public string Identifier { get; set; }
    }

    public class Property
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsOptional { get; set; }
        public string References { get; set; }
        public string ReferencesIdentifier { get; set; }
        public string RelationType { get; set; }
    }
}
