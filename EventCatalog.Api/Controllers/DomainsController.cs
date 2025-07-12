using EventCatalog.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventCatalog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DomainsController : ControllerBase
    {
        private readonly CatalogService _catalogService;

        public DomainsController()
        {
            _catalogService = new CatalogService("examples/default");
        }

        [HttpGet]
        public IEnumerable<Domain> Get()
        {
            return _catalogService.GetDomains();
        }
    }
}
