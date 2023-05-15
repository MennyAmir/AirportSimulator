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

        public AirportService(AirportDbContext airportDbContext) {
            _airportDbContext = airportDbContext;
        }

        public void AddAirplane(AirplaneDbDto a) {
            Console.WriteLine("I'm working");

            _airportDbContext.Airplanes.Add(a);
            _airportDbContext.SaveChanges();
        }


/*        public bool Save() {
            var saved = _airportDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }*/
    }
}
