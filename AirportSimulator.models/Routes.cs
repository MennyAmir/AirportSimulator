using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.models
{
    public class Routes {
        public LinkedList<Station> TakeoffRoute = new LinkedList<Station>();
        public LinkedList<Station> LandRoute = new LinkedList<Station>();


        /*  Station waitingToTakeoff = new Station(0, "waitingToTakeoff");
          Station waitingToLand = new Station(10, "waitingToLand");*/
        Station s1 = new Station(1, "before landing");
        Station s2 = new Station(2, "Preparing for landing");
        Station s3 = new Station(3, "landing");
        Station s4 = new Station(4, "on track");
        Station s5 = new Station(5, "landed");
        Station s6 = new Station(6, "in the terminal");
        Station s7 = new Station(7, "in the terminal");
        Station s8 = new Station(8, "before the appearance");
        Station s9 = new Station(9, "teekoff");


        /*        LinkedListNode<Station> waitingToTakeof = new LinkedListNode<Station>(new Station(0, "waitingToTakeoff"));
        */




        public Routes() {





            /*            LandRoute.AddFirst(waitingToLand);
            */
            LandRoute.AddFirst(s1);
            LandRoute.AddLast(s2);
            LandRoute.AddLast(s3);
            LandRoute.AddLast(s4);
            LandRoute.AddLast(s5);
            LandRoute.AddLast(s6);
            LandRoute.AddLast(s7);


            /*            TakeoffRoute.AddFirst(waitingToTakeoff);
            */
            TakeoffRoute.AddFirst(s6);
            TakeoffRoute.AddLast(s7);
            TakeoffRoute.AddLast(s8);
            TakeoffRoute.AddLast(s4);
            TakeoffRoute.AddLast(s9);




        }


        public LinkedList<Station> ReturnRoutes(FlightType type) {

            if (type == FlightType.Incoming)
            {
                return LandRoute;
            }
            else
            {
                return TakeoffRoute;
            }

        }

        public List<Station> GetStations() {
            List<Station> stations = new List<Station>();
            stations.Add(s1);
            stations.Add(s2);
            stations.Add(s3);
            stations.Add(s4);
            stations.Add(s5);
            stations.Add(s6);
            stations.Add(s7);
            stations.Add(s8);
            stations.Add(s9);

            return stations;
        }
    }


}
 

  