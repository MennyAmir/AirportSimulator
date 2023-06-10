 
using AirportSimulator.data.DTO;
using AirportSimulator.models;

namespace AirportSimulator.services.Interfaces
{
    public interface IFlightHubs
    {

        public Task SendFlight(AirplaneDbDto flight);
        public Task SendAllFlights(AirplaneDbDto[] flights);
        public Task SendLandingFlights(AirplaneDbDto[] flights);

        public Task SendTakeOffFlights(AirplaneDbDto[] flights);

        public Task SendStateOfStations(StationDbDto[] states);
        public Task SendStateOfStation(StationDbDto state);
        public Task SendVisit(Visit visit);
    }
}