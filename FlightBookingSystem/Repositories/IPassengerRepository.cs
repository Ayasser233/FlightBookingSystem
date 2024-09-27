using FlightBookingSystem.Models;

namespace FlightBookingSystem.Repositories
{
    public interface IPassengerRepository : IRepository<Passenger>
    {
        Task<IEnumerable<Passenger>> GetPassengersByBookingIdAsync(int bookingId);
       
    }
}
