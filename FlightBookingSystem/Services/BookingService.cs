using FlightBookingSystem.DTOs;
using FlightBookingSystem.Models;
using FlightBookingSystem.Repositories;

namespace FlightBookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly AirLineDBcontext _context;
        private readonly IFlightRepository _flightRepository;

        public BookingService(AirLineDBcontext context, IFlightRepository flightRepository)
        {
            _context = context;
            _flightRepository = flightRepository;
        }

        public async Task CreateBooking(int flightId, List<PassengerDto> passengers)
        {
            var flight = await _flightRepository.GetById(flightId);

            if (flight == null || flight.AvailableSeats < passengers.Count)
            {
                throw new Exception("Not enough seats available.");
            }

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

            // Update booked seats instead of available seats
            flight.BookedSeats += passengers.Count; // Increase booked seats

            _context.Bookings.Add(booking);
            _context.Flights.Update(flight); // Update flight with new seat count

            await _context.SaveChangesAsync();
        }
    }

}
