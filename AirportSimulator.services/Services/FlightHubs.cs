using AirportSimulator.data.DTO;
using AirportSimulator.models;
using AirportSimulator.services.Interfaces;
using Microsoft.AspNetCore.SignalR;
namespace AirportSimulator.services
{

    public class FlightHubs : Hub, IFlightHubs
    {

        public FlightHubs()
        {
            
        }
    }
} 