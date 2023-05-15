using AirportSimulator.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.data.DTO
{
    public class StationDbDto
    {
        public int id { get; set; }
        public string Name { get; set; }
        public Airplane? AirplaneInSta { get; set; }
        public StationDbDto() { }
    }
}
