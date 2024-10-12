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

        public async Task CreateBooking(int Userid, int flightId, List<PassengerDto> passengers ,PaymentDto paymentDto)
        {
            var flight = await _flightRepository.GetById(flightId);
            

            if (flight == null || flight.AvailableSeats < passengers.Count)
            {
                throw new Exception("Not enough seats available.");
            }


            var ALL = new 
            {
                UserId = Userid,
                FlightId = flightId,
                Passengers = passengers.Select(p => new Passenger
                {
                    FullName = p.FullName,
                    PassportNumber = p.PassportNumber,
                    Nationality=p.Nationality,
                    DateOfBirth=p.DateOfBirth,     
                }).ToList(),
                BookingDate = DateTime.UtcNow,
                Flight = flight,
                TotalPrice = (int)(flight.BasePrice * passengers.Count),
                 
                Amount = paymentDto.TotalPrice,
                PaymentMethod = paymentDto.PaymentMethod,
                PaymentDate = paymentDto.ExpiryDate
            };

            var booking = new Booking
            {
                UserId = ALL.UserId,
                FlightId = ALL.FlightId,
                 ArrivalAirport=flight.ArrivalAirport,
                
                Status=BookingStatus.Pending ,
                
                Passengers = ALL.Passengers,
                BookingDate = DateTime.UtcNow,
                Flight = flight,
                TotalPrice = (int)(flight.BasePrice * passengers.Count)
            };
          

            
            // Update booked seats instead of available seats
            flight.BookedSeats += passengers.Count; // Increase booked seats
            flight.AvailableSeats -= passengers.Count;
            _context.Bookings.Add(booking);
            _context.Flights.Update(flight); // Update flight with new seat count
            
            await _context.SaveChangesAsync();
        }
    }

}
