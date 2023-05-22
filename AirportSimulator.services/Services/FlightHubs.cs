

namespace AirportSimulator.services
{

    public class FlightHubs : Hub, IFlightHubs
    {
        public async Task SendFlight(PlaneDTO flight)// or dto or new obj include *current station*
        {
            await Clients.All.SendAsync("UpdateFlight", flight);
        }

        public async Task SendAllFlights(PlaneDTO[] flights)
        {
            await Clients.All.SendAsync("UpdateAllFlights", flights);
        }
        public async Task SendStateOfStations(StateOfStations states)//StationDbDto[] states
        {
            await Clients.All.SendAsync("UpdateStates", states);
        }
        public async Task SendStateOfStation(StationDbDto state)//Single station transfer
        {
            await Clients.All.SendAsync("UpdateState", state);
        }
    }
}