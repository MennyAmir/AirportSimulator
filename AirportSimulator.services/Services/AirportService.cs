using AirportSimulator.data;
using AirportSimulator.data.DTO;
using AirportSimulator.models;
using AirportSimulator.services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AirportSimulator.services.Services
{
    public class AirportService : IAirportService {
        private readonly AirportDbContext _airportDbContext;
        private readonly IHubContext<FlightHubs> _hub;

        public AirportService(AirportDbContext airportDbContext, IHubContext<FlightHubs> hub) {
            _airportDbContext = airportDbContext;
            _hub = hub;
        }

        public async void AddAirplane(AirplaneDbDto a) {
            Console.WriteLine("I'm working");

            _airportDbContext.Airplanes.Add(a);
            _airportDbContext.SaveChanges();

            await _hub.Clients.All.SendAsync("sendAirplane", $"the airplane: {a.FlightNumber} to {a.Country} -- {a.TypeOfFlight}");
        }

        public void AddVisit(Visit visit) {
            _airportDbContext.Visits.Add(visit);
            _airportDbContext.SaveChanges();
        }
        public async void UpdeateVisit(Airplane a, DateTime ET) {
            a.CurrentVisit.ExitTime = ET;

            _airportDbContext.Visits.Update(a.CurrentVisit);
            _airportDbContext.SaveChanges();

            await _hub.Clients.All.SendAsync
                ("sendVisit", $"the airplane: {a.FlightNumber} was at the station: {a.CurrentVisit.StationId} from {a.CurrentVisit.EntryTime:hh:mm:ss.F} to {a.CurrentVisit.ExitTime:hh:mm:ss.F}"); 
        }

        public void ReportStations(StationDbDto[] stations) {
            foreach (var stat in stations)
            {
                _airportDbContext.StationsStatus.Add(stat);
            }
            _airportDbContext.SaveChanges();

            Console.WriteLine("stations send");
        }



        public Station GetStationByID(int id) {
            StationDbDto stationDbDto = _airportDbContext.Stations.First(s => s.id == id);
            return new Station(stationDbDto.id, stationDbDto.Name);

        }


        public List<Tuple<Station?, Station?>> GetTakeoffRoute() {
            Station s4 = GetStationByID(4);
            Station s6 = GetStationByID(6);
            Station s7 = GetStationByID(7);
            Station s8 = GetStationByID(8);
            Station s9 = GetStationByID(9);

            List<Tuple<Station?, Station?>> TakeoffRoute = new List<Tuple<Station?, Station?>>();

            TakeoffRoute.Add(new Tuple<Station?, Station?>(null, s6));
            TakeoffRoute.Add(new Tuple<Station?, Station?>(null, s7));
            TakeoffRoute.Add(new Tuple<Station?, Station?>(s6, s8));
            TakeoffRoute.Add(new Tuple<Station?, Station?>(s7, s8));
            TakeoffRoute.Add(new Tuple<Station?, Station?>(s8, s4));
            TakeoffRoute.Add(new Tuple<Station?, Station?>(s4, s9));
            TakeoffRoute.Add(new Tuple<Station?, Station?>(s9, null));

            return TakeoffRoute;
        }
        public List<Tuple<Station?, Station?>> GetLandRoute() {
            Station s1 = GetStationByID(1);
            Station s2 = GetStationByID(2);
            Station s3 = GetStationByID(3);
            Station s4 = GetStationByID(4);
            Station s5 = GetStationByID(5);
            Station s6 = GetStationByID(6);
            Station s7 = GetStationByID(7);


            List<Tuple<Station?, Station?>> LandRoute = new List<Tuple<Station?, Station?>>();

            LandRoute.Add(new Tuple<Station?, Station?>(null, s1));
            LandRoute.Add(new Tuple<Station?, Station?>(s1, s2));
            LandRoute.Add(new Tuple<Station?, Station?>(s2, s3));
            LandRoute.Add(new Tuple<Station?, Station?>(s3, s4));
            LandRoute.Add(new Tuple<Station?, Station?>(s4, s5));
            LandRoute.Add(new Tuple<Station?, Station?>(s5, s6));
            LandRoute.Add(new Tuple<Station?, Station?>(s5, s7));
            LandRoute.Add(new Tuple<Station?, Station?>(s6, null));
            LandRoute.Add(new Tuple<Station?, Station?>(s7, null));

            return LandRoute;
        }


        public List<Tuple<Station?, Station?>> GetRoute(FlightType flightType) {
            if (flightType != FlightType.Incoming && flightType != FlightType.Departing) { return null; }
            if (flightType == FlightType.Incoming)
            {
                return GetLandRoute();
            }
            else
                return GetTakeoffRoute();

        }






    }





    /*        public bool Save() {
                var saved = _airportDbContext.SaveChanges();
                return saved > 0 ? true : false;
            }*/
}

