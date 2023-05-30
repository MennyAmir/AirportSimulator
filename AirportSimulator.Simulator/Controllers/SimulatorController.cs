using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportSimulator.Simulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulatorController : ControllerBase {
        private readonly Models.Simulator simulator;

        public SimulatorController(Models.Simulator simulator) {
            this.simulator = simulator;
        }

        [HttpGet("start")]
        public void Start() {
            _ = simulator.SandAirplans();

        }
    }
}
