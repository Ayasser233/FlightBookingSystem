using FlightBookingSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.ModelView
{
    public class Bookingpage1
    {


        [Required, MaxLength(100)]
        public string DepartureAirport { get; set; }

        [Required, MaxLength(100)]
        public string ArrivalAirport { get; set; }

        public DateTime TimeOfFlight;

        [EnumDataType(typeof(Class))]
        public Class PassengerClass;

        ICollection<Passenger> NumberOfPassengers ;
    }
    public enum Class
    {
        Economy,
        Business,
        First
    }
}
