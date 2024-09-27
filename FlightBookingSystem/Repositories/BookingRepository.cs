using FlightBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingSystem.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AirLineDBcontext context;

        public BookingRepository(AirLineDBcontext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await context.Bookings.ToListAsync();
        }

        public async Task<Booking> GetById(int id)
        {
            return await context.Bookings.FindAsync(id);
        }

        public async Task Add(Booking booking)
        {
            await context.Bookings.AddAsync(booking);
            await context.SaveChangesAsync();
        }

        public async Task Update(Booking booking)
        {
            context.Bookings.Update(booking);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var booking = await GetById(id);
            if (booking != null)
            {
                context.Bookings.Remove(booking);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Flight)
                .Include(b => b.Passengers)
                .ToListAsync();
        }
        public async Task<IEnumerable<Booking>> GetBookingsByFlightIdAsync(int flightId)
        {
            return await context.Bookings
                .Where(b => b.FlightId == flightId)
                .Include(b => b.Flight)
                .Include(b => b.Passengers)
                .ToListAsync();
        }
        public async Task<IEnumerable<Booking>> GetPendingBookingsAsync()
        {
            return await context.Bookings
                .Where(b => b.Status == BookingStatus.Pending)
                .Include(b => b.Flight)
                .Include(b => b.Passengers)
                .ToListAsync();
        }

    }
}
