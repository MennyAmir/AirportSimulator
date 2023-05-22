using AirportSimulator.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.data.DTO
{
    public class AirplaneDbDto
    {
        public int id { get; set; }
        public string FlightNumber { get; set; }
        public string Country { get; set; }
        public string TypeOfFlight { get; set; }

        public AirplaneDbDto() {

        }
    
    }
}
