using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SatelliteDemoSnapshots.DemoSnapshots.Common.Entities;
using SatelliteDemoSnapshots.DemoSnapshots.DL.DAO.Interfaces;
using System.Threading.Tasks;

namespace SatelliteDemoSnapshots.DemoSnapshots.BL.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoSnapshotController : ControllerBase
    {
        private readonly ILogger<DemoSnapshotController> _logger;
        private readonly IRepository<DemoSnapshot> _demoSnapshotRepository;

        public DemoSnapshotController(ILogger<DemoSnapshotController> logger, IRepository<DemoSnapshot> demoSnapshotRepository)
        {
            _logger = logger;
            _demoSnapshotRepository = demoSnapshotRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _demoSnapshotRepository.GetAllAsync());
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<DemoSnapshot> GetAsync(int id)
        {
            return await _demoSnapshotRepository.ReadAsync(id);
        }

        [HttpPost]
        public async Task<int> PostAsync([FromBody] DemoSnapshot demoSnapshot)
        {
            return await _demoSnapshotRepository.CreateAsync(demoSnapshot);
        }

        [HttpPut("{id}")]
        public async Task<int> PutAsync(int id, [FromBody] DemoSnapshot demoSnapshot)
        {
            return await _demoSnapshotRepository.UpdateAsync(demoSnapshot);
        }

        [HttpDelete("{id}")]
        public async Task<int> DeleteAsync(int id)
        {
            return await _demoSnapshotRepository.DeleteAsync(id);
        }
    }
}