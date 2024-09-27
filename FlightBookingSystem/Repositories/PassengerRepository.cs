using FlightBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingSystem.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly AirLineDBcontext context;
        public PassengerRepository(AirLineDBcontext context) 
        {
            this.context = context;
        }

        public async Task<IEnumerable<Passenger>> GetAllAsync() 
        {
            return await context.Passengers.ToListAsync();
        }

        public async Task<Passenger> GetById(int id)
        {
            return await context.Passengers.FindAsync(id);
        }

        public async Task Add(Passenger passenger) 
        {
            await context.Passengers.AddAsync(passenger);
            await context.SaveChangesAsync();
        }

        public async Task Update(Passenger passenger) 
        {
            context.Passengers.Update(passenger);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id) 
        {
            var passenger = await GetById(id);
            if (passenger != null) 
            { 
                context.Passengers.Remove(passenger);
                await context.SaveChangesAsync();   
            }
        }

        public async Task<IEnumerable<Passenger>> GetPassengersByBookingIdAsync(int bookingId)
        {
            return await context.Passengers
                .Where(p => p.BookingId == bookingId)
                .ToListAsync();
        }
    }
}
