
using AirportSimulator.data;
using AirportSimulator.data.DTO;
using AirportSimulator.models;
using AirportSimulator.services.Interfaces;
using AirportSimulator.services.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.services
{
    public class ControlTower : IControlTower
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /*        private readonly IAirportService _airportService;
*/

        public List<Airplane> airplanes { get; set; }
        public Routes routes { get; set; }
        public ConcurrentQueue<Airplane> DepartingAirplanes { get; set; }

        public ConcurrentQueue<Airplane> IncomingAirplanes { get; set; }

        public ControlTower(/*IAirportService airportService*/ IServiceScopeFactory serviceScopeFactory)
        {
            /*_airportService = airportService;*/


            airplanes = new List<Airplane>();
            routes = new Routes();
            DepartingAirplanes = new ConcurrentQueue<Airplane>();
            IncomingAirplanes = new ConcurrentQueue<Airplane>();
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public Airplane AddFlight(Airplane a) {
            AddingFlightData(a);

            Console.WriteLine(a.id + "," + a.FlightNumber);

            using IServiceScope scope = AddingPlaneToDB(a);

            airplanes.Add(a);

            for (int i = 0; i < airplanes.Count; i++)
            {
                Console.WriteLine(airplanes[i].FlightNumber);
            }

            return a;
        }

        private IServiceScope AddingPlaneToDB(Airplane a) {
            AirplaneDbDto airplaneDbDto = new AirplaneDbDto()
            {
                FlightNumber = a.FlightNumber,
                Country = a.Country,
                TypeOfFlight = a.TypeOfFlight,
            };
            var scope = _serviceScopeFactory.CreateScope();
            var _airportService = scope.ServiceProvider.GetRequiredService<IAirportService>();
            _airportService.AddAirplane(airplaneDbDto);
            return scope;
        }

        private void AddingFlightData(Airplane a) {
            a.Stations = routes.ReturnRoutes(a.TypeOfFlight);
/*            a.CurrentStation = a.Stations!.First;
*/            if (a.TypeOfFlight == FlightType.Incoming)
            {
                IncomingAirplanes.Enqueue(a);
            }
            else if (a.TypeOfFlight == FlightType.Departing)
            {
                DepartingAirplanes.Enqueue(a);
            }
        }

        public IEnumerable<Airplane> GetAirplanes() {  return airplanes; }


    }
}
