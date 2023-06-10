using AirportSimulator.data.DTO;
using AirportSimulator.models;
using AirportSimulator.services.Interfaces;
using Microsoft.AspNetCore.SignalR;
namespace AirportSimulator.services
{

    public class FlightHubs : Hub, IFlightHubs
    {
        public async Task SendFlight(AirplaneDbDto flight)// or dto or new obj include *current station*
        {
            if (Clients !=  null)
            {
                await Clients.All.SendAsync("UpdateFlight", flight);
            }
        }

        public async Task SendAllFlights(AirplaneDbDto[] flights)
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("UpdateAllFlights", flights);
            }
        }

        public async Task SendLandingFlights(AirplaneDbDto[] flights)
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("UpdateLandingFlights", flights);
            }
        }

        public async Task SendTakeOffFlights(AirplaneDbDto[] flights)
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("UpdateTakeOffFlights", flights);
            }
        }

        public async Task SendStateOfStations(StationDbDto[] states)//StationDbDto[] states
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("UpdateStates", states);
            }
        }

        public async Task SendStateOfStation(StationDbDto state)//Single station transfer
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("UpdateState", state);
            }
        }

        public async Task SendVisit(Visit visit)
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("ReportVisit", visit);
            }
        }
    }
} 