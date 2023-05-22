using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
*/        public ConcurrentQueue<Airplane> WaitingToEnter { get; set; }
        [JsonIgnore]
        public SemaphoreSlim StationBusy { get; set; }
        public bool Available { get; set; }

        public Station(int id, string Name)
        {
            this.id = id;
            this.Name = Name;
            Available = true;
            StationBusy = new SemaphoreSlim(1);
        }


        public void AddToWaiting(Airplane airplane) => WaitingToEnter.Enqueue(airplane);

        public void LockSt() => StationBusy.Wait();

        public void UnlockSt() => StationBusy.Release();

    }
}
