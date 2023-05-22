
namespace AirportSimulator.services.Interfaces
{
    public interface IFlightHubs
    {

        public Task SendFlight(PlaneDTO flight);
        public Task SendAllFlights(PlaneDTO[] flights);
        public Task SendStateOfStations(StateOfStations states);
        public Task SendStateOfStation(StationDbDto state);

    }
}