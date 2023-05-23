using AirportSimulator.data;
using AirportSimulator.data.DTO;
using AirportSimulator.models;
using AirportSimulator.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.services.Services
{
    public class AirportService : IAirportService
    {
        private readonly AirportDbContext _airportDbContext;
        private readonly IFlightHubs _flightHubs;

        public AirportService(AirportDbContext airportDbContext, IFlightHubs flightHubs) {
            _airportDbContext = airportDbContext;
            _flightHubs = flightHubs;
        }

        public void AddAirplane(AirplaneDbDto a) {
            Console.WriteLine("I'm working");

            _airportDbContext.Airplanes.Add(a);
            _airportDbContext.SaveChanges();
            _flightHubs.SendFlight(a);
        }

        public void AddVisit(Visit visit)
        {
            _airportDbContext.Visits.Add(visit);
            _airportDbContext.SaveChanges();

            _flightHubs.SendVisit(visit);
            Console.WriteLine("visit send");
        }

        public void ReportStations(StationDbDto[] stations)
        {
            _airportDbContext.StationsStatus.Add(stations);
            _airportDbContext.SaveChanges();

            _flightHubs.SendStateOfStations(stations);
            Console.WriteLine("stations send");
        }


        public void AddLandingAirplane(AirplaneDbDto flights)
        {
            _airportDbContext.PlannedLandings.Add(flights);
            _airportDbContext.SaveChanges();
            _flightHubs.SendTakeOffFlights(flights);
            Console.WriteLine("PlannedLandings reoprt");
        }

        public void AddPlannedTakeOffAirplane(AirplaneDbDto[] flights)
        {
            _airportDbContext.PlannedTakeOff.Add(flights);
            _airportDbContext.SaveChanges();
            _flightHubs.SendLandingFlights(flights);
            Console.WriteLine("PlannedTakeOff reoprt");
        }



        /*        public bool Save() {
                    var saved = _airportDbContext.SaveChanges();
                    return saved > 0 ? true : false;
                }*/
    }
}
