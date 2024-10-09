using FlightBookingSystem.DTOs;
using FlightBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingSystem.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AirLineDBcontext context;
        public PaymentRepository(AirLineDBcontext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await context.Payments.ToListAsync();
        }

        public async Task<Payment> GetById(int id)
        {
            return await context.Payments.FindAsync(id);
        }

        public async Task Add(Payment payment)
        {
            await context.Payments.AddAsync(payment);
            await context.SaveChangesAsync();
        }

        public async Task Update(Payment payment)
        {
            context.Payments.Update(payment);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var payment = await GetById(id);
            if (payment != null)
            {
                context.Payments.Remove(payment);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Payment> GetPaymentsByBookingIdAsync(int bookingId)
        {
            return await context.Payments.FirstOrDefaultAsync(p => p.BookingId == bookingId);
        }

        public async Task<bool> ProcessPayment(PaymentDto paymentDto)
        {
            // Implement your payment processing logic here
            // Example:
            var payment = new Payment
            {
                Amount = paymentDto.TotalPrice,
                PaymentMethod = paymentDto.PaymentMethod,
                // Other properties...
                PaymentDate = DateTime.Now,
                Status = PaymentStatus.Pending,
                TransactionId = null
            };

            await context.Payments.AddAsync(payment);
            await context.SaveChangesAsync();
            return true; // Return true if payment processing is successful
        }
    }
}
