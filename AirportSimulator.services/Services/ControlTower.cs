
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

namespace AirportSimulator.services
{
    public class ControlTower : IControlTower
    {
        private static System.Timers.Timer ctTimer;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        /*        private readonly IAirportService _airportService;
*/

        public List<Airplane> airplanes { get; set; }
        public Routes routes { get; set; }

        public List<Station> stations { get; set; }
        public ConcurrentQueue<Airplane> DepartingAirplanes { get; set; }

        public ConcurrentQueue<Airplane> IncomingAirplanes { get; set; }
        public List<Airplane> FinishedRoute { get; set; }

        public ControlTower(/*IAirportService airportService*/ IServiceScopeFactory serviceScopeFactory) {
            /*_airportService = airportService;*/

            airplanes = new List<Airplane>();
            stations = new List<Station>();
            routes = new Routes();
            stations = routes.GetStations();
            DepartingAirplanes = new ConcurrentQueue<Airplane>();
            IncomingAirplanes = new ConcurrentQueue<Airplane>();
            FinishedRoute = new List<Airplane>();
            this._serviceScopeFactory = serviceScopeFactory;
            Task.Run(() => RunRunway());
            /*SetTimer();*/
        }


        public Airplane AddFlight(Airplane a) {

            AddingFlightData(a);

            Console.WriteLine(a.id + "," + a.FlightNumber);
            AddingPlaneToDB(a);



            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < airplanes.Count; i++)
            {

                Console.WriteLine("IN THE LIST:" + airplanes[i].FlightNumber);
            }
            Console.ResetColor();

            return a;
        }

        private IServiceScope AddingPlaneToDB(Airplane a) {
            AirplaneDbDto airplaneDbDto = new AirplaneDbDto()
            {
                FlightNumber = a.FlightNumber,
                Country = a.Country,
                TypeOfFlight = a.TypeOfFlight.ToString(),
            };
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            var _airportService = scope.ServiceProvider.GetRequiredService<IAirportService>();
            _airportService.AddAirplane(airplaneDbDto);

            return scope;
        }

        private void AddingFlightData(Airplane a) {
            a.Stations = routes.ReturnRoutes(a.TypeOfFlight);
            a.StationsT = routes.ReturnRoutesT(a.TypeOfFlight);
            airplanes.Add(a);


            /*            a.CurrentStation = a.Stations!.First;
            */
           /* if (a.TypeOfFlight == FlightType.Incoming)
            {
                IncomingAirplanes.Enqueue(a);
            }
            else if (a.TypeOfFlight == FlightType.Departing)
            {
                DepartingAirplanes.Enqueue(a);
            }*/
        }

        public IEnumerable<Airplane> GetAirplanes() { return airplanes; }


        public void RunRunway() {
           /* Task.Run(async () =>
            {
                while (true)
                {
                    WaitingForLanding(IncomingAirplanes);
                    WaitingForDeparting(DepartingAirplanes);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("!!RunRunway Waiting Works!!");
                    Console.ResetColor();
                    await Task.Delay(1000);
                }
            });*/

            Task.Run(async () =>
            {

                while (true)
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("!!RunRunway Works!!");
                    Console.ResetColor();
                    List<Airplane> finishedRoute = new List<Airplane>();

                   /* foreach (var a in airplanes)
                    {
                        if (a.CurrentStation == null)
                        {
                            finishedRoute.Add(a);
                        }
                        else
                        {
                            if (a.CurrentStation.Next.Value.Available == true)
                            {
                                a.CurrentStation.Next.Value.LockSt();

                                AirplaneCrossing(a);

                            }
                            if (a.CurrentStation.Next == null)
                            {
                                AirplaneCrossing(a);

                            }
                        }

                    }*/

 



                    foreach (var a in airplanes)
                    {
                        Station? s = a.CurrentStationT;
                        Station? nextStation = null;
                        List<Station> asd = new List<Station>();

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

                        if (asd != null && asd.Count > 0)
                        {
                            if (asd.Count == 1) 
                            {
                                nextStation = asd.First();

                            }
                             if (asd.Count == 2)
                            {
                                if (asd[0].Available == false && asd[1].Available == false) { break; }
                                if (asd[0].Available == true) { nextStation = asd.First(); }
                                if (asd[1].Available == true) { nextStation = asd.Last(); }
                            }
                        }

                        if (nextStation == null) { break; }
                        nextStation.LockSt();
                        // מטוס מגיע לכאן ויודע מה התחנה הבאה שלו 
                        // אם אין לו תחנה הבאה == הוא סיים את המסלול הוא לא יכנס 
                        // בפונקציה הבאה צריך לבדוק עם הוא נמצא בתחנה ואם כן להוציא אותו מהתכנה ולשחרר אותה 
                        // ואז את כולם צריך להכניס לתחנה הבאה שלהם ולעדכן הכל 
                        AirplaneCrossingToNextST(a,nextStation);


                    }

                    while (finishedRoute.Count > 0)
                    {
                        Airplane a = finishedRoute.First();
                        airplanes.Remove(a);
                        finishedRoute.Remove(a);
                        Console.WriteLine($"Airplan {a.FlightNumber} deleted from the main list");
                    }

                    await Task.Delay(1000);


                    /* foreach  (var s in stations)
                     {
                         if (s.Available = true && s.WaitingToEnter.Count != 0)
                         {
                             _ = Task.Run(() =>
                             {
                                 StationCrossing(s);
                             }
                             );
                         }
                     }*/
                }

            });
        }

        private void AirplaneCrossingToNextST(Airplane a, Station nextStation) {
            if (a.CurrentStationT != null)
            {
                Station previousStation = a.CurrentStationT;
                previousStation.Available = true;
/*                previousStation.AirplaneInSta = a;*/              
                previousStation.UnlockSt();
            }

            /*nextStation.AirplaneInSta = a;*/
            nextStation.Available = false;
            a.CurrentStationT = nextStation;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"The plane {a.FlightNumber} entered the station {a.CurrentStationT.Name}");
            Console.ResetColor();

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
    }
}
