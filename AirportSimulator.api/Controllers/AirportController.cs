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
        public Airplane AddIncomingAirplane(int id) {
            return _controlTower.AddFlight(new Airplane(id, FlightType.Incoming));
        }

        [HttpGet("Departing/{id}")]
        public Airplane AddDepartingAirplane(int id) {
            return _controlTower.AddFlight(new Airplane(id, FlightType.Departing));
        }
    }
}
