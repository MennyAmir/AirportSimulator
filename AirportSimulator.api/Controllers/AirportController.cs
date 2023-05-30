using AirportSimulator.models;
using AirportSimulator.services;
using AirportSimulator.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportSimulator.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly IControlTower _controlTower;



        public AirportController(IControlTower controlTower )
        {
            _controlTower = controlTower;

        }


        [HttpGet("GetAirplanes")]
        public IEnumerable<Airplane> GetAirplanes() {
            return _controlTower.GetAirplanes();
        }


        [HttpGet("Incoming/{id}")]
        public IActionResult AddIncomingAirplane(int id) {
            if (_controlTower.airplanes.Count > 10)
            {
                return BadRequest();
            }
            _controlTower.AddFlight(new Airplane(id, FlightType.Incoming));
            return Ok();
        }

        [HttpGet("Departing/{id}")]
        public IActionResult AddDepartingAirplane(int id) {
            if (_controlTower.airplanes.Count > 10)
            {
                return BadRequest();
            }
            _controlTower.AddFlight(new Airplane(id, FlightType.Departing));
            return Ok();
        }
    }
}
