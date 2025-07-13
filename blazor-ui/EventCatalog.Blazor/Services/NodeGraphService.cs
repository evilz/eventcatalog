using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventCatalog.Blazor.Models;

namespace EventCatalog.Blazor.Services
{
    public class NodeGraphService
    {
        private readonly ContentService _contentService;

        public NodeGraphService(ContentService contentService)
        {
            _contentService = contentService;
        }

        public async Task<(List<Node<object>>, List<Edge>)> GetNodesAndEdgesForDomainContextMap()
        {
            var nodes = new List<Node<object>>();
            var edges = new List<Edge>();

            var domains = await _contentService.GetCollection<Domain>("domains");
            var services = await _contentService.GetCollection<Service>("services");

            foreach (var domain in domains)
            {
                var nodeId = Utils.GenerateIdForNode(domain.data);
                var domainServices = domain.data.Services?.Select(s => services.FirstOrDefault(c => c.data.Id == s.Id && c.data.Version == s.Version)).Where(s => s != null).ToList();

                var servicesCount = domainServices?.Count() ?? 0;
                const int servicesPerRow = 1;
                const int serviceWidth = 330;
                const int serviceHeight = 100;
                const int padding = 40;

                var domainWidth = serviceWidth * servicesPerRow;
                var domainHeight = serviceHeight * servicesCount + padding * 4;

                var index = domains.IndexOf(domain);
                const int domainsPerRow = 2;
                var rowIndex = index / domainsPerRow;
                var colIndex = index % domainsPerRow;

                nodes.Add(new Node<object>
                {
                    Id = nodeId,
                    Type = "group",
                    Position = new Position { X = colIndex * (domainWidth + 400), Y = rowIndex * (domainHeight + 300) },
                    Data = new { label = domain.data.Name, domain = domain.data }
                });

                if (domainServices != null)
                {
                    foreach (var service in domainServices)
                    {
                        var serviceNodeId = Utils.GenerateIdForNode(service.data);
                        var serviceIndex = domainServices.IndexOf(service);
                        var row = serviceIndex / servicesPerRow;
                        var col = serviceIndex % servicesPerRow;
                        const int serviceMargin = 25;
                        const int titleHeight = 20;
                        var xPosition = padding + col * (serviceWidth + serviceMargin) + 20;
                        var yPosition = padding + row * (serviceHeight + serviceMargin) + titleHeight;

                        nodes.Add(new Node<object>
                        {
                            Id = serviceNodeId,
                            Type = "services",
                            Position = new Position { X = xPosition, Y = yPosition },
                            Data = new { service = service.data }
                        });

                        var receives = service.data.Receives;
                        if (receives != null)
                        {
                            foreach (var receive in receives)
                            {
                                var producers = GetProducersOfMessage(services, receive);
                                foreach (var producer in producers)
                                {
                                    var isSameDomain = domainServices.Any(ds => ds.data.Id == producer.data.Id && ds.data.Version == producer.data.Version);
                                    if (!isSameDomain)
                                    {
                                        edges.Add(new Edge
                                        {
                                            Id = Utils.GenerateIdForEdge(producer.data, service.data),
                                            Source = Utils.GenerateIdForNode(producer.data),
                                            Target = Utils.GenerateIdForNode(service.data),
                                            Label = "sends to"
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return (nodes, edges);
        }

        public async Task<(List<Node<object>>, List<Edge>)> GetNodesAndEdgesForFlow(string id, string version)
        {
            var nodes = new List<Node<object>>();
            var edges = new List<Edge>();

            var flows = await _contentService.GetCollection<Flow>("flows");
            var flow = flows.FirstOrDefault(f => f.data.Id == id && f.data.Version == version);

            if (flow == null)
            {
                return (nodes, edges);
            }

            var steps = flow.data.Steps;

            if (steps != null)
            {
                foreach (var step in steps)
                {
                    var node = new Node<object>
                    {
                        Id = $"step-{step.Id}",
                        Position = new Position { X = 0, Y = 0 },
                        Data = new { step }
                    };

                    if (step.Service != null)
                    {
                        node.Type = "services";
                    }
                    else if (step.Flow != null)
                    {
                        node.Type = "flows";
                    }
                    else if (step.Message != null)
                    {
                        node.Type = "messages";
                    }
                    else
                    {
                        node.Type = "step";
                    }

                    nodes.Add(node);

                    var paths = step.NextSteps ?? new List<NextStep>();
                    if (step.NextStep != null)
                    {
                        paths.Add(step.NextStep);
                    }

                    foreach (var path in paths)
                    {
                        edges.Add(new Edge
                        {
                            Id = $"step-{step.Id}-step-{path.Id}",
                            Source = $"step-{step.Id}",
                            Target = $"step-{path.Id}",
                            Label = path.Label,
                            Animated = true,
                            Type = "flow-edge"
                        });
                    }
                }
            }

            return (nodes, edges);
        }

        public async Task<(List<Node<object>>, List<Edge>)> GetNodesAndEdgesForMessage(string id, string version, string collectionName)
        {
            var nodes = new List<Node<object>>();
            var edges = new List<Edge>();

            var messages = await _contentService.GetCollection<Message>(collectionName);
            var message = messages.FirstOrDefault(m => m.data.Id == id && m.data.Version == version);

            if (message == null)
            {
                return (nodes, edges);
            }

            var services = await _contentService.GetCollection<Service>("services");

            var producers = message.data.Producers?.Select(p => services.FirstOrDefault(s => s.data.Id == p.Id && s.data.Version == p.Version)).Where(p => p != null).ToList();
            var consumers = message.data.Consumers?.Select(c => services.FirstOrDefault(s => s.data.Id == c.Id && s.data.Version == c.Version)).Where(c => c != null).ToList();

            nodes.Add(new Node<object>
            {
                Id = Utils.GenerateIdForNode(message.data),
                Type = message.collection,
                Position = new Position { X = 0, Y = 0 },
                Data = new { message = message.data }
            });

            if (producers != null)
            {
                foreach (var producer in producers)
                {
                    nodes.Add(new Node<object>
                    {
                        Id = Utils.GenerateIdForNode(producer.data),
                        Type = "services",
                        Position = new Position { X = 0, Y = 0 },
                        Data = new { service = producer.data }
                    });

                    edges.Add(new Edge
                    {
                        Id = Utils.GenerateIdForEdge(producer.data, message.data),
                        Source = Utils.GenerateIdForNode(producer.data),
                        Target = Utils.GenerateIdForNode(message.data),
                        Label = "produces"
                    });
                }
            }

            if (consumers != null)
            {
                foreach (var consumer in consumers)
                {
                    nodes.Add(new Node<object>
                    {
                        Id = Utils.GenerateIdForNode(consumer.data),
                        Type = "services",
                        Position = new Position { X = 0, Y = 0 },
                        Data = new { service = consumer.data }
                    });

                    edges.Add(new Edge
                    {
                        Id = Utils.GenerateIdForEdge(message.data, consumer.data),
                        Source = Utils.GenerateIdForNode(message.data),
                        Target = Utils.GenerateIdForNode(consumer.data),
                        Label = "consumes"
                    });
                }
            }

            return (nodes, edges);
        }

        public async Task<(List<Node<object>>, List<Edge>)> GetNodesAndEdgesForDomain(string id, string version)
        {
            var nodes = new List<Node<object>>();
            var edges = new List<Edge>();

            var domains = await _contentService.GetCollection<Domain>("domains");
            var domain = domains.FirstOrDefault(d => d.data.Id == id && d.data.Version == version);

            if (domain == null)
            {
                return (nodes, edges);
            }

            if (domain.data.Services != null)
            {
                foreach (var serviceSummary in domain.data.Services)
                {
                    var (serviceNodes, serviceEdges) = await GetNodesAndEdgesForService(serviceSummary.Id, serviceSummary.Version);
                    nodes.AddRange(serviceNodes);
                    edges.AddRange(serviceEdges);
                }
            }

            if (domain.data.Domains != null)
            {
                foreach (var subDomainSummary in domain.data.Domains)
                {
                    var (subDomainNodes, subDomainEdges) = await GetNodesAndEdgesForDomain(subDomainSummary.Id, subDomainSummary.Version);
                    nodes.AddRange(subDomainNodes);
                    edges.AddRange(subDomainEdges);
                }
            }

            return (nodes, edges);
        }

        private List<Collection<Service>> GetProducersOfMessage(List<Collection<Service>> services, MessageSummary message)
        {
            return services.Where(s => s.data.Sends != null && s.data.Sends.Any(m => m.Id == message.Id && m.Version == message.Version)).ToList();
        }

        public async Task<(List<Node<object>>, List<Edge>)> GetNodesAndEdgesForService(string id, string version)
        {
            var nodes = new List<Node<object>>();
            var edges = new List<Edge>();

            var services = await _contentService.GetCollection<Service>("services");
            var service = services.FirstOrDefault(s => s.data.Id == id && s.data.Version == version);

            if (service == null)
            {
                return (nodes, edges);
            }

            var messages = await _contentService.GetCollection<Message>("events");
            messages.AddRange(await _contentService.GetCollection<Message>("commands"));
            messages.AddRange(await _contentService.GetCollection<Message>("queries"));

            var receives = service.data.Receives?.Select(m => messages.FirstOrDefault(msg => msg.data.Id == m.Id && msg.data.Version == m.Version)).Where(m => m != null).ToList();
            var sends = service.data.Sends?.Select(m => messages.FirstOrDefault(msg => msg.data.Id == m.Id && msg.data.Version == m.Version)).Where(m => m != null).ToList();

            nodes.Add(new Node<object>
            {
                Id = Utils.GenerateIdForNode(service.data),
                Type = "services",
                Position = new Position { X = 0, Y = 0 },
                Data = new { service = service.data }
            });

            if (receives != null)
            {
                foreach (var receive in receives)
                {
                    nodes.Add(new Node<object>
                    {
                        Id = Utils.GenerateIdForNode(receive.data),
                        Type = receive.collection,
                        Position = new Position { X = 0, Y = 0 },
                        Data = new { message = receive.data }
                    });

                    edges.Add(new Edge
                    {
                        Id = Utils.GenerateIdForEdge(receive.data, service.data),
                        Source = Utils.GenerateIdForNode(receive.data),
                        Target = Utils.GenerateIdForNode(service.data),
                        Label = "receives"
                    });
                }
            }

            if (sends != null)
            {
                foreach (var send in sends)
                {
                    nodes.Add(new Node<object>
                    {
                        Id = Utils.GenerateIdForNode(send.data),
                        Type = send.collection,
                        Position = new Position { X = 0, Y = 0 },
                        Data = new { message = send.data }
                    });

                    edges.Add(new Edge
                    {
                        Id = Utils.GenerateIdForEdge(service.data, send.data),
                        Source = Utils.GenerateIdForNode(service.data),
                        Target = Utils.GenerateIdForNode(send.data),
                        Label = "sends"
                    });
                }
            }

            return (nodes, edges);
        }
    }
}
