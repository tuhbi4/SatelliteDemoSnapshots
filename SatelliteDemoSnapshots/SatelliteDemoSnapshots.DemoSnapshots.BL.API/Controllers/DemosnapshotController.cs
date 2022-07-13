using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SatelliteDemoSnapshots.DemoSnapshots.BL.API.Models;
using SatelliteDemoSnapshots.DemoSnapshots.Common.Entities;
using SatelliteDemoSnapshots.DemoSnapshots.DL.DAO.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SatelliteDemoSnapshots.DemoSnapshots.BL.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoSnapshotController : ControllerBase
    {
        private readonly ILogger<DemoSnapshotController> _logger;
        private readonly IRepository<DemoSnapshot> _demoSnapshotRepository;
        private readonly IMapper mapper;

        public DemoSnapshotController(ILogger<DemoSnapshotController> logger, IRepository<DemoSnapshot> demoSnapshotRepository, IMapper mapper)
        {
            _logger = logger;
            _demoSnapshotRepository = demoSnapshotRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] string query)
        {
            IReadOnlyList<DemoSnapshot> demoSnapshots = await _demoSnapshotRepository.GetAllAsync(query);

            return Ok(mapper.Map<List<DemoSnapshot>, List<DemoSnapshotModel>>(demoSnapshots.ToList()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            DemoSnapshot demoSnapshot = await _demoSnapshotRepository.ReadAsync(id);
            DemoSnapshotModel demoSnapshotModel = mapper.Map<DemoSnapshotModel>(demoSnapshot);

            return Ok(demoSnapshotModel);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] DemoSnapshotModel demoSnapshotModel)
        {
            if (ModelState.IsValid)
            {
                DemoSnapshot demoSnapshot = mapper.Map<DemoSnapshot>(demoSnapshotModel);

                return Ok(await _demoSnapshotRepository.CreateAsync(demoSnapshot));
            }

            return ValidationProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] DemoSnapshotModel demoSnapshotModel)
        {
            if (ModelState.IsValid)
            {
                DemoSnapshot demoSnapshot = mapper.Map<DemoSnapshot>(demoSnapshotModel);

                return Ok(await _demoSnapshotRepository.UpdateAsync(demoSnapshot));
            }

            return ValidationProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            return Ok(await _demoSnapshotRepository.DeleteAsync(id));
        }

        public IActionResult Error()
        {
            return NotFound();
        }
    }
}