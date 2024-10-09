using FlightBookingSystem.DTOs;
using FlightBookingSystem.Models;

namespace FlightBookingSystem.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<Payment> GetPaymentsByBookingIdAsync(int bookingId);
        Task<bool> ProcessPayment(PaymentDto paymentDto);
    }
}
