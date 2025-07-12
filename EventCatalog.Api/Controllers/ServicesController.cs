using EventCatalog.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventCatalog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly CatalogService _catalogService;

        public ServicesController()
        {
            _catalogService = new CatalogService("examples/default");
        }

        [HttpGet]
        public IEnumerable<Service> Get()
        {
            return _catalogService.GetServices();
        }
    }
}
