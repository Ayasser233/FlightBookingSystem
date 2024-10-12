using FlightBookingSystem.DTOs;

namespace FlightBookingSystem.Services
{
    public interface IBookingService
    {
        Task CreateBooking(int UserID, int flightId, List<PassengerDto> passengers,PaymentDto paymentDto);
    }

}
