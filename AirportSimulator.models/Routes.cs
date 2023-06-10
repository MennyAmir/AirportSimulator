using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.models
{
    public class Routes {

        public List<Tuple<Station?, Station?>> TakeoffRouteT = new List<Tuple<Station?, Station?>>();
        public List<Tuple<Station?, Station?>> LandRouteT = new List<Tuple<Station?, Station?>>();

        Station s1 = new Station(1, "before landing");
        Station s2 = new Station(2, "Preparing for landing");
        Station s3 = new Station(3, "landing");
        Station s4 = new Station(4, "on track");
        Station s5 = new Station(5, "landed");
        Station s6 = new Station(6, "in the terminal 1");
        Station s7 = new Station(7, "in the terminal 2");
        Station s8 = new Station(8, "before the appearance");
        Station s9 = new Station(9, "teekoff");

        public Routes() {






            LandRouteT.Add(new Tuple<Station?, Station?>(null, s1));
            LandRouteT.Add(new Tuple<Station?, Station?>(s1, s2));
            LandRouteT.Add(new Tuple<Station?, Station?>(s2, s3));
            LandRouteT.Add(new Tuple<Station?, Station?>(s3, s4));
            LandRouteT.Add(new Tuple<Station?, Station?>(s4, s5));
            LandRouteT.Add(new Tuple<Station?, Station?>(s5, s6));
            LandRouteT.Add(new Tuple<Station?, Station?>(s5, s7));
            LandRouteT.Add(new Tuple<Station?, Station?>(s6, null));
            LandRouteT.Add(new Tuple<Station?, Station?>(s7, null));




            TakeoffRouteT.Add(new Tuple<Station?, Station?>(null, s6));
            TakeoffRouteT.Add(new Tuple<Station?, Station?>(null, s7));
            TakeoffRouteT.Add(new Tuple<Station?, Station?>(s6, s8));
            TakeoffRouteT.Add(new Tuple<Station?, Station?>(s7, s8));
            TakeoffRouteT.Add(new Tuple<Station?, Station?>(s8, s4));
            TakeoffRouteT.Add(new Tuple<Station?, Station?>(s4, s9));
            TakeoffRouteT.Add(new Tuple<Station?, Station?>(s9, null));
        }


        public List<Tuple<Station?, Station>> ReturnRoutesT(FlightType type) {

            if (type == FlightType.Incoming)
            {
                return LandRouteT;
            }
            else
            {
                return TakeoffRouteT;
            }

        }

    }


}
 

  