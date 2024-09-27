using FlightBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingSystem.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly AirLineDBcontext context;
        public FlightRepository(AirLineDBcontext context) 
        { 
            this.context = context;
        }

        public async Task<IEnumerable<Flight>> GetAllAsync() 
        {
            return await context.Flights.ToListAsync();
        }

        public async Task<Flight> GetById(int id)
        {
            return await context.Flights.FindAsync(id);
        }

        public async Task Add(Flight flight) 
        {
            await context.Flights.AddAsync(flight);
            await context.SaveChangesAsync();
        }

        public async Task Update(Flight flight) 
        {
            context.Flights.Update(flight);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id) 
        {
            var flight = await GetById(id);
            if (flight != null) 
            { 
                context.Flights.Remove(flight);
                await context.SaveChangesAsync();   
            }
        }

        public async Task<IEnumerable<Flight>> SearchFlightsAsync(string departureAirport, string ArrivalAirport, DateTime departureDate, int noOfPassengers)
        {
            return await context.Flights
                .Where(f => 
                        f.DepartureAirport == departureAirport &&
                        f.ArrivalAirport == ArrivalAirport &&
                        f.DepartureTime == departureDate &&
                        f.AvailableSeats >= noOfPassengers)
                .ToListAsync();
        }
    }
}
