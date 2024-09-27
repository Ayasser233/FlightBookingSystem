using FlightBookingSystem.Models;

namespace FlightBookingSystem.Repositories
{
    public interface IFlightRepository : IRepository<Flight>
    {
        Task<IEnumerable<Flight>> SearchFlightsAsync(string departureAirport, string ArrivalAirport, DateTime departureDate, int noOfPassengers);
    }
}
