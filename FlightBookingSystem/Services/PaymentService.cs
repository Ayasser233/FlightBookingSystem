using FlightBookingSystem.DTOs;

namespace FlightBookingSystem.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<bool> ProcessPayment(PaymentDto paymentDto)
        {
            // Simulate payment processing (this can be integrated with a real payment gateway)
            if (string.IsNullOrWhiteSpace(paymentDto.CardNumber)
                || paymentDto.TotalPrice <= 0
                || paymentDto.ExpiryDate < DateTime.Now)
            {
                return false;
            }

            // Assume payment processing was successful
            return await Task.FromResult(true);
        }
    }

}
