using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AirportSimulator.models
{
    public class Station
    {
        public int id { get; set; }
        public string Name { get; set; }
/*        public Airplane? AirplaneInSta { get; set; }
*/      
 
        [JsonIgnore]
        [NotMapped]
        public SemaphoreSlim StationBusy { get; set; }
        public bool Available { get; set; }

        public Station(int id, string Name)
        {
            this.id = id;
            this.Name = Name;
            Available = true;
            StationBusy = new SemaphoreSlim(1);
        }



        public void LockSt() => StationBusy.Wait();

        public void UnlockSt() => StationBusy.Release();

    }
}
