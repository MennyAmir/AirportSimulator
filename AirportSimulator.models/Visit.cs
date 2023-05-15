using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.models
{
    public class Visit
    {
        public int Id { get; set; }
        public int AirplameId { get; set; }
        public int StationId { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }


        public Visit(int AirplameId, int StationId)
        {
            this.AirplameId = AirplameId;
            this.StationId = StationId;
            EntryTime = DateTime.Now;
        }
    }
}
