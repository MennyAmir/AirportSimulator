using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.models
{
    public class Station
    {
        public int id { get; set; }
        public string Name { get; set; }
        public Airplane? AirplaneInSta { get; set; }

        public Station(int id, string Name)
        {
            this.id = id;
            this.Name = Name;

        }

    }
}
