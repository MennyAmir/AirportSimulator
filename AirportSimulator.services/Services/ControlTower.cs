﻿
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
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace AirportSimulator.services
{
    public class ControlTower : IControlTower {

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public List<Airplane> airplanes { get; set; }
        public Routes routes { get; set; }

        public List<Airplane> FinishedRoute { get; set; }

        public ControlTower(/*IAirportService airportService*/ IServiceScopeFactory serviceScopeFactory) {
            /*_airportService = airportService;*/

            airplanes = new List<Airplane>();
/*            routes = new Routes();
*/
            FinishedRoute = new List<Airplane>();
            this._serviceScopeFactory = serviceScopeFactory;
            Task.Run(() => RunRunway());
        }


        public Airplane AddFlight(Airplane a) {

            AddingRoute(a);

            Console.WriteLine(a.id + "," + a.FlightNumber);
            airplanes.Add(a);
            AddingPlaneToDB(a);

            return a;
        }

        private void AddingPlaneToDB(Airplane a) {
            AirplaneDbDto airplaneDbDto = new AirplaneDbDto()
            {
                FlightNumber = a.FlightNumber,
                Country = a.Country,
                TypeOfFlight = a.TypeOfFlight.ToString(),
            };
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            var _airportService = scope.ServiceProvider.GetRequiredService<IAirportService>();
            _airportService.AddAirplane(airplaneDbDto);

        }

        private void AddingRoute(Airplane a) {


            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            var _airportService = scope.ServiceProvider.GetRequiredService<IAirportService>();
            a.StationsT = _airportService.GetRoute(a.TypeOfFlight);


/*            a.StationsT = routes.ReturnRoutesT(a.TypeOfFlight);
*/
        }

        public IEnumerable<Airplane> GetAirplanes() { return airplanes; }


        public void RunRunway() {


            Task.Run(async () =>
            {

                while (true)
                {

                    List<Airplane> finishedRoute = new List<Airplane>();


                    foreach (var a in airplanes)  
                    {
                        if (a.Passing == true) break;

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"AIRPLAME {a.FlightNumber} START");
                        Console.ResetColor();

                        Station? s = a.CurrentStationT;
                        Station? nextStation = null;
                        List<Station> asd = new List<Station>();

                        GetNextPossibleStations(finishedRoute, a, s, asd);

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"GetNextPossibleStations {a.FlightNumber} WORKS");
                        Console.ResetColor();

                        nextStation = GetNextOpenStation(asd);

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"GetNextOpenStation {a.FlightNumber} WORKS");
                        Console.ResetColor();

                        /*                        if (nextStation == null)
                                                {
                                                    s!.UnlockSt();
                                                    s.Available = true;
                                                    break; 
                                                }*/

                        if (nextStation != null) { nextStation.LockSt(); }

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"before {a.FlightNumber} AirplaneCrossingToNextST");
                        Console.ResetColor();
                        
                        AirplaneCrossingToNextST(a, nextStation);

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"AirplaneCrossingToNextST {a.FlightNumber} WORKS");
                        Console.ResetColor();
                    }

                    while (finishedRoute.Count > 0)
                    {
                        Airplane a = finishedRoute.First();
                        airplanes.Remove(a);
                        finishedRoute.Remove(a);
                        Console.WriteLine($"Airplan {a.FlightNumber} deleted from the main list");
                    }

                    await Task.Delay(1000);
                }

            });
        }


        private static void GetNextPossibleStations(List<Airplane> finishedRoute, Airplane a, Station? s, List<Station> asd) {
            foreach (var ts in a.StationsT)
            {


                if (s == null && ts.Item1 == null) 
                                                   
                {
                    if (a.TypeOfFlight == FlightType.Incoming)
                    {
                        asd.Add(ts.Item2);
                        break;
                    }
                    if (a.TypeOfFlight == FlightType.Departing)
                    {
                        asd.Add(ts.Item2);

                    }
                }
                else if (s == ts.Item1) 
                {
                    if (ts.Item2 != null) 
                        asd.Add(ts.Item2);
                    else 
                    {
                        finishedRoute.Add(a);
                        break;
                    }


                }


            }
        }
        private Station GetNextOpenStation(List<Station> asd) {
            Station? s = null;
            if (asd != null && asd.Count > 0)
            {

                if (asd.Count == 1)
                {
                    s = asd.First();

                }
                if (asd.Count == 2)
                {
                    if (asd[0].Available == false && asd[1].Available == false) { return null; }
                    else if (asd[0].Available == true) { s = asd.First(); }
                    else if (asd[1].Available == true) { s = asd.Last(); }
                }
                return s;
            }
            return null;
        }

        private void AirplaneCrossingToNextST(Airplane a, Station? nextStation) {
            if (a.CurrentStationT != null)
            {
                ExitPreviousStation(a);
            }


            if (nextStation != null)
            {
                EntreNextStation(a, nextStation);
            }

        }

        private void ExitPreviousStation(Airplane a) {
            Station previousStation = a.CurrentStationT!;
            previousStation.Available = true;
            /*previousStation.AirplaneInSta = a;*/
            UpdeateVisit(a, previousStation);
            previousStation.UnlockSt();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"The plane {a.FlightNumber} left the station {a.CurrentStationT.Name}");
            Console.ResetColor();
        }
        private async void EntreNextStation(Airplane a, Station? nextStation) {
            /*nextStation.AirplaneInSta = a;*/
            nextStation.Available = false;
            a.CurrentStationT = nextStation;
            CreateVisit(a, nextStation);
            await a.CrossingStation();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"The plane {a.FlightNumber} entered the station {a.CurrentStationT.Name}");
            Console.ResetColor();
        }



        private void CreateVisit(Airplane a, Station enteredStation) {

            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            var _airportService = scope.ServiceProvider.GetRequiredService<IAirportService>();
            Visit newVisit = new Visit(a.id, enteredStation.id);
            a.CurrentVisit = newVisit;
            _airportService.AddVisit(newVisit);
        }

        private void UpdeateVisit(Airplane a, Station enteredStation) {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            var _airportService = scope.ServiceProvider.GetRequiredService<IAirportService>();
            DateTime ExitTime = DateTime.Now;
            _airportService.UpdeateVisit(a, ExitTime);
            /*_airportService.UpdeateVisit(a, ExitTime);*/

        }




        
    }
}
/*        private void AirplaneCrossing(Airplane a) {


            // לשחרר בתחנה הנוכחית שהיא פנויה ולהוציא ממנה את המטוס
            *//*            a.CurrentStation.Value.AirplaneInSta = null;
            *//*
            a.CurrentStation.Value.Available = true;
            a.CurrentStation.Value.UnlockSt();


            // אם הגיע לתחנה אחרונה
            if (a.CurrentStation.Next == null)
            {
                a.CurrentStation = null;
                FinishedRoute.Add(a);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"The plane {a.FlightNumber} finished a route");
                Console.ResetColor();
            }
            else
            {
                // להכניס את המטוס ברשימה הבאה ולהחריז שהוא טפוס 
                *//*                a.CurrentStation.Next.Value.AirplaneInSta = a;
                *//*
                a.CurrentStation.Next.Value.Available = false;
                //לשנות במטוס עצמו שהוא עבר לתחנה הבאה 
                a.CurrentStation = a.CurrentStation.Next;
                using (IAirportService service = new AirportService())
                {
                    Visit newVisit = new Visit(a.id, a.CurrentStation); 
                    service.AddVisit(newVisit);
                }

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"The plane {a.FlightNumber} entered the station {a.CurrentStation.Value.Name}");
                Console.ResetColor();
            }
        }
*/
/* private void StationCrossing(Station s) {
     s.StationBusy.Wait();


     // להכניס את המטוס הראשון שמחכה 
     // להוציא את המטוס שנכנס 
     // למצוא את התכנה הבאה הטובה 
     // להכניס אותו לרשימה (ות) של התכנה הבאה 
 }*/

/*        public void WaitingForLanding(ConcurrentQueue<Airplane> airplansForLandidin) {

            if (airplansForLandidin.Count == 0) { return; }

            if (routes.LandRoute.First!.Value.Available == true)
            {
                if (airplansForLandidin.TryDequeue(out Airplane a))
                {


                    a.CurrentStation = routes.LandRoute.First;
                    a.CurrentStation.Value.LockSt();

                    *//*                    a.CurrentStation.Value.AirplaneInSta = a;
                    *//*
                    a.CurrentStation.Value.Available = false;
                    airplanes.Add(a);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"The plane {a.FlightNumber} entered the station {a.CurrentStation.Value.Name}");
                    Console.ResetColor();
                }
            }
        }


        public void WaitingForDeparting(ConcurrentQueue<Airplane> airplansForDepartind) {

            if (airplansForDepartind.Count == 0) { return; }

            if (routes.TakeoffRoute.First!.Value.Available == true)
            {
                if (airplansForDepartind.TryDequeue(out Airplane a))
                {

                    a.CurrentStation = routes.TakeoffRoute.First;
                    a.CurrentStation.Value.LockSt();

                    *//*                    a.CurrentStation.Value.AirplaneInSta = a;
                    *//*
                    a.CurrentStation.Value.Available = false;
                    airplanes.Add(a);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"The plane {a.FlightNumber} entered the station {a.CurrentStation.Value.Name}");
                    Console.ResetColor();
                }
            }

            *//*  if (routes.TakeoffRoute.First!.Value.AirplaneInSta == null || routes.TakeoffRoute.First.Next!.Value.AirplaneInSta == null)
              {
                  if (airplansForDepartind.TryDequeue(out Airplane a))

                  {
                      if (!(routes.TakeoffRoute.First.Value.AirplaneInSta == null))
                      {
                          routes.TakeoffRoute.First.Next!.Value.StationBusy.Wait();
                          a.CurrentStation = routes.TakeoffRoute.First.Next;
                          a.CurrentStation.Value.AirplaneInSta = a;
                          a.CurrentStation.Value.Available = false;
                          airplanes.Add(a);

                          Console.ForegroundColor = ConsoleColor.DarkYellow;
                          Console.WriteLine($"The plane {a.FlightNumber} entered the station {a.CurrentStation.Value.Name}");
                          Console.ResetColor();
                      }
                      if (!(routes.TakeoffRoute.First.Next.Value.AirplaneInSta == null))
                      {
                          routes.TakeoffRoute.First!.Value.StationBusy.Wait();
                          a.CurrentStation = routes.TakeoffRoute.First;
                          a.CurrentStation.Value.AirplaneInSta = a;
                          a.CurrentStation.Value.Available = false;
                          airplanes.Add(a);

                          Console.ForegroundColor = ConsoleColor.DarkYellow;
                          Console.WriteLine($"The plane {a.FlightNumber} entered the station {a.CurrentStation.Value.Name}");
                          Console.ResetColor();
                      }


                  }
              }*//*
        }*/






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

