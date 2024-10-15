using FlightBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using FlightBookingSystem.DTOs;

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

        

       
        public async Task<IEnumerable<Flight>> SearchFlightsAsync(string departureAirport, string arrivalAirport, DateTime departureDate, int noOfPassengers)
        {
            return await context.Flights
                .Where(f => f.DepartureAirport == departureAirport
                            && f.ArrivalAirport == arrivalAirport
                            && f.DepartureTime.Date == departureDate.Date  // Use Date comparison for exact match
                            && f.AvailableSeats >= noOfPassengers)  // Ensure enough available seats
                .ToListAsync();
        }

        // Method 2: GetAvailableFlights
        public async Task<IEnumerable<Flight>> GetAvailableFlights(string fromAirport, string toAirport, DateTime flightDate, SeatClass seatClass)
        {
            return await context.Flights
                .Where(f => f.DepartureAirport == fromAirport
                            && f.ArrivalAirport == toAirport
                            && f.DepartureTime.Date == flightDate.Date 
                            && f.AvailableSeats > 0  )
                .ToListAsync();
        }


    }
}
