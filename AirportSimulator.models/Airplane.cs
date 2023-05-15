using System.ComponentModel.DataAnnotations.Schema;

namespace AirportSimulator.models
{
    public enum AirplaneStatse
    {
        beforeLanding = 1,
        onLanding,
        onRunway,
        droppingPassengers,
        Land,
        pickingPassengers,
        waitingForTakeoff,
        Departure
    }

    public enum FlightType { Incoming, Departing }

    public class Airplane
    {
        public int id { get; set; }
        public string FlightNumber { get; set; }
        public string Country { get; set; }
        public Visit? CurrentVisit { get; set; }
        public FlightType TypeOfFlight { get; set; }
        [NotMapped]
        public AirplaneStatse Statse { get; set; }
        [NotMapped]
        public LinkedList<Station>? Stations { get; set; }
        [NotMapped]
        public LinkedListNode<Station>? CurrentStation { get; set; }
        [NotMapped]
        public static string[,] States => new string[,] { { "London" ,"EGKK" }, { "Geneva" ,"LSGG" }, {"Lisbon"," LSGG"},{ "Barcelona", "LEBL" }, { "Napoli", " LIRN" },{ "Paris", " LFPB" },{ "Athena", " LGAV" },{ "Strasbourg", " LFST" } };
        [NotMapped]
        private static readonly Random _getRandom = new Random();


        public Airplane(int id, FlightType TypeOfFlight) {
            int rnd = _getRandom.Next(0, 7);
            this.id = id;
            FlightNumber = States[rnd, 1] + id.ToString();
            Country = States[rnd, 0];
            this.TypeOfFlight = TypeOfFlight;
        }

        /*        public void  RoutesControl(Airplane a) {
                    a.Stations = Routes.ReturnRoutes(a.type);
                }*/





    }


}
