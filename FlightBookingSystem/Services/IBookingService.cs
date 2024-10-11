using FlightBookingSystem.DTOs;

namespace FlightBookingSystem.Services
{
    public interface IBookingService
    {
        Task CreateBooking(int flightId, List<PassengerDto> passengers,PaymentDto paymentDto);
    }

}
