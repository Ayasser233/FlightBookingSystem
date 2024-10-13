using FlightBookingSystem.DTOs;
using FlightBookingSystem.Models;
using FlightBookingSystem.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingSystem.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AirLineDBcontext context;
        IFlightRepository FlightRepository;

        public BookingRepository(AirLineDBcontext context, IFlightRepository flightRepository)
        {
            this.context = context;
            this.FlightRepository = flightRepository;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await context.Bookings
                .Include(b => b.Flight)
                .Include(b => b.Passengers)
                .ToListAsync();
        }

        public async Task<Booking> GetById(int id)
        {
            return await context.Bookings
                .Include(b => b.Flight)
                .Include(b => b.Passengers)
                .FirstOrDefaultAsync(b => b.BookingId == id);
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
        public async Task CreateBooking(int flightId, List<PassengerDto> passengers)
        {
            // Step 1: Get the flight by its ID
            var flight = await FlightRepository.GetById(flightId);

            if (flight == null)
            {
                throw new Exception("Flight not found");
            }

            // Step 2: Check if there are enough available seats
            if (flight.AvailableSeats < passengers.Count)
            {
                throw new Exception("Not enough seats available.");
            }

            // Step 3: Create the booking
            var booking = new Booking
            {
                FlightId = flightId,
                Passengers = passengers.Select(p => new Passenger
                {
                    FullName = p.FullName,
                    PassportNumber = p.PassportNumber
                }).ToList(),
                BookingDate = DateTime.UtcNow,
                TotalPrice = (int)(flight.BasePrice * passengers.Count)
            };

            // Step 4: Update the booked seats
            flight.AvailableSeats -= passengers.Count;

            // Step 5: Save the booking and update the flight in the database
            context.Bookings.Add(booking);
            context.Flights.Update(flight);

            await context.SaveChangesAsync();
        }
    }
}
