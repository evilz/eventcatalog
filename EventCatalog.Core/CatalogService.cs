using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventCatalog.Core
{
    public class CatalogService
    {
        private readonly string _basePath;

        public CatalogService(string basePath)
        {
            _basePath = basePath;
        }

        public IEnumerable<Domain> GetDomains()
        {
            var domainsPath = Path.Combine(_basePath, "domains");
            if (!Directory.Exists(domainsPath))
            {
                return Enumerable.Empty<Domain>();
            }

            var domainDirectories = Directory.GetDirectories(domainsPath);
            var domains = new List<Domain>();

            foreach (var domainDirectory in domainDirectories)
            {
                var indexPath = Path.Combine(domainDirectory, "index.md");
                if (File.Exists(indexPath))
                {
                    var markdown = File.ReadAllText(indexPath);
                    var (frontMatter, _) = MarkdownParser.Parse<Domain>(markdown);
                    domains.Add(frontMatter);
                }
            }

            return domains;
        }

        public IEnumerable<Service> GetServices()
        {
            var servicesPath = Path.Combine(_basePath, "services");
            if (!Directory.Exists(servicesPath))
            {
                return Enumerable.Empty<Service>();
            }

            var serviceFiles = Directory.GetFiles(servicesPath, "*.md", SearchOption.AllDirectories);
            var services = new List<Service>();

            foreach (var serviceFile in serviceFiles)
            {
                var markdown = File.ReadAllText(serviceFile);
                var (frontMatter, _) = MarkdownParser.Parse<Service>(markdown);
                services.Add(frontMatter);
            }

            return services;
        }

        public IEnumerable<Event> GetEvents()
        {
            var eventsPath = Path.Combine(_basePath, "events");
            if (!Directory.Exists(eventsPath))
            {
                return Enumerable.Empty<Event>();
            }

            var eventFiles = Directory.GetFiles(eventsPath, "*.md", SearchOption.AllDirectories);
            var events = new List<Event>();

            foreach (var eventFile in eventFiles)
            {
                var markdown = File.ReadAllText(eventFile);
                var (frontMatter, _) = MarkdownParser.Parse<Event>(markdown);
                events.Add(frontMatter);
            }

            return events;
        }
    }
}
