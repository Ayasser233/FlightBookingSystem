using FlightBookingSystem.DTOs;
using FlightBookingSystem.Models;

namespace FlightBookingSystem.Repositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<Booking>> GetBookingsByFlightIdAsync(int flightId);
        Task<IEnumerable<Booking>> GetPendingBookingsAsync();
        Task CreateBooking(int flightId, List<PassengerDto> passengers);
    }
}
