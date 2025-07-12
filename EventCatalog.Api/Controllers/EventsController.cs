using EventCatalog.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventCatalog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly CatalogService _catalogService;

        public EventsController()
        {
            _catalogService = new CatalogService("examples/default");
        }

        [HttpGet]
        public IEnumerable<Event> Get()
        {
            return _catalogService.GetEvents();
        }
    }
}
