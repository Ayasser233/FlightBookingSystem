using FlightBookingSystem.DTOs;

namespace FlightBookingSystem.Services
{
    public interface IPaymentService
    {
        Task<bool> ProcessPayment(PaymentDto paymentDto);
    }

}
