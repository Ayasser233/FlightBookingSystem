using FlightBookingSystem.DTOs;
using FlightBookingSystem.Models;

namespace FlightBookingSystem.ModelView
{
    public class Step2ViewModel
    {
        public IEnumerable<Flight> Flights { get; set; }
        public BookingDto BookingDetails { get; set; }
    }

}
