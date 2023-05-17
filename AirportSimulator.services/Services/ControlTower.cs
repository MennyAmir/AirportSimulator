
using AirportSimulator.data;
using AirportSimulator.data.DTO;
using System.Timers;
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
using System.Threading;

namespace AirportSimulator.services
{
    public class ControlTower : IControlTower {
        private static System.Timers.Timer ctTimer;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        /*        private readonly IAirportService _airportService;
*/

        public List<Airplane> airplanes { get; set; }
        public Routes routes { get; set; }
        public ConcurrentQueue<Airplane> DepartingAirplanes { get; set; }

        public ConcurrentQueue<Airplane> IncomingAirplanes { get; set; }

        public ControlTower(/*IAirportService airportService*/ IServiceScopeFactory serviceScopeFactory) {
            /*_airportService = airportService;*/

            airplanes = new List<Airplane>();
            routes = new Routes();
            DepartingAirplanes = new ConcurrentQueue<Airplane>();
            IncomingAirplanes = new ConcurrentQueue<Airplane>();
            this._serviceScopeFactory = serviceScopeFactory;
            SetTimer();
        }


        public void SetTimer() {
            ctTimer = new System.Timers.Timer(2000);
            ctTimer.Start();
            ctTimer.Elapsed += CtTimer_Elapsed;
            Console.WriteLine("CtTimer Works");
        }

        private void CtTimer_Elapsed(object? sender, ElapsedEventArgs e) {
            WaitingForLanding(IncomingAirplanes);
            WaitingForDeparting(DepartingAirplanes);
            Console.WriteLine("CtTimer stil Works");
        }

        public Airplane AddFlight(Airplane a) {
            AddingFlightData(a);

            Console.WriteLine(a.id + "," + a.FlightNumber);

            using IServiceScope scope = AddingPlaneToDB(a);

            airplanes.Add(a);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            for (int i = 0; i < airplanes.Count; i++)
            {

                Console.WriteLine(airplanes[i].FlightNumber);
            }
            Console.ResetColor();

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


        public void WaitingForLanding(ConcurrentQueue<Airplane> a) {
            if (routes.LandRoute.First!.Value.AirplaneInSta == null)
            {
                if (a.TryDequeue(out Airplane airplane))
                {
                    routes.LandRoute.First.Value.AirplaneInSta = airplane;
                    routes.LandRoute.First.Value.Available = false;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"The plane {airplane.FlightNumber} entered the station {routes.LandRoute.First.Value.Name}");
                    Console.ResetColor();

                }
            }
        }


        public void WaitingForDeparting(ConcurrentQueue<Airplane> a) {
            if (routes.LandRoute.First!.Value.AirplaneInSta == null || routes.LandRoute.First.Next!.Value.AirplaneInSta == null)
            {
                if (a.TryDequeue(out Airplane airplane))
                {
                    if (!(routes.LandRoute.First.Value.AirplaneInSta == null))
                    {
                        routes.LandRoute.First.Next.Value.AirplaneInSta = airplane;
                        routes.LandRoute.First.Next.Value.Available = false;
                    }
                    if (!(routes.LandRoute.First.Next.Value.AirplaneInSta == null))
                    {
                        routes.LandRoute.First.Value.AirplaneInSta = airplane;
                        routes.LandRoute.First.Value.Available = false;
                    }
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"The plane {airplane.FlightNumber} entered the station {routes.LandRoute.First.Value.Name}");
                    Console.ResetColor();

                }
            }
        }



      /*  public void NewFlightSlot() {
            // Create a new timer that runs every minute
            var timer = new Timer(_ =>
            {
                // Check if S1 and S6 are available
                if (S1.Occupied == null && S6.Occupied == null)
                {
                    // Do something
                    // ...
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }*/
    }
}
