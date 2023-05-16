using Microsoft.AspNetCore.SignalR;
using AirportSimulator.models;
using AirportSimulator.data.DTO;

namespace AirportSimulator.api.Hubs
{
    public class FlightHubs : Hub
    {
        public async Task SendFlight(Airplane flight )// or dto or new obj include *current station*
        {
            await Clients.All.SendAsync("ReceiveMessage", flight);
        }

        public async Task SendAllFlights(Airplane[] flights)
        {
            await Clients.All.SendAsync("UpdateAllFlights", flights);
        }
        public async Task SendStateOfStations(StateOfStations states)//StationDbDto[] states
        {
            await Clients.All.SendAsync("UpdateState", states);
        }
        public async Task SendStateOfStation(StationDbDto state)//Single station transfer
        {
            await Clients.All.SendAsync("UpdateState", state);
        }
    }
}
