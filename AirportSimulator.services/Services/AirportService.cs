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
        public async void UpdeateVisit(Airplane a, DateTime ET) {
            a.CurrentVisit.ExitTime = ET;
            
            _airportDbContext.Visits.Update(a.CurrentVisit);
            _airportDbContext.SaveChanges();

            await _flightHubs.SendVisit(a.CurrentVisit); // גם פה צריך לעשות שרק יעדכן 
            Console.WriteLine("visit send");
        }

        public void ReportStations(StationDbDto[] stations)
        {
            foreach (var stat in stations)
            {
                _airportDbContext.StationsStatus.Add(stat);
            }
            _airportDbContext.SaveChanges();

            _flightHubs.SendStateOfStations(stations);
            Console.WriteLine("stations send");
        }

        public void AddLandingAirplane(AirplaneDbDto flight)
        {
            _airportDbContext.PlannedLandings.Add(flight);
            _airportDbContext.SaveChanges();
            AirplaneDbDto[] dbDto = _airportDbContext.PlannedLandings.ToArray();
            _flightHubs.SendTakeOffFlights(dbDto);
            Console.WriteLine("PlannedLandings reoprt");
        }

        public void AddPlannedTakeOffAirplane(AirplaneDbDto flight)
        {
            _airportDbContext.PlannedTakeOff.Add(flight);
            _airportDbContext.SaveChanges();
            AirplaneDbDto[] dbDto = _airportDbContext.PlannedTakeOff.ToArray();
            _flightHubs.SendLandingFlights(dbDto);
            Console.WriteLine("PlannedTakeOff reoprt");
        }





        /*        public bool Save() {
                    var saved = _airportDbContext.SaveChanges();
                    return saved > 0 ? true : false;
                }*/
    }
}
