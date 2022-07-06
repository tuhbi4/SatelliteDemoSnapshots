using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SatelliteDemoSnapshots.DemoSnapshots.Common.Entities;
using System;
using System.Collections.Generic;

namespace SatelliteDemoSnapshots.DemoSnapshots.BL.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemosnapshotController : ControllerBase
    {
        private readonly ILogger<DemosnapshotController> _logger;

        public DemosnapshotController(ILogger<DemosnapshotController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Demosnapshot> Get()
        {
            throw new NotSupportedException();
        }
    }
}
