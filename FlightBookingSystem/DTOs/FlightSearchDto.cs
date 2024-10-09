using FlightBookingSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.DTOs
{
    public class FlightSearchDto
    {
        public int FlightId { get; set; }
        [Required]
        public string FromAirport { get; set; }

        [Required]
        public string ToAirport { get; set; }

        [Required]
        public int NumberOfPassengers { get; set; }

        [Required]
        public SeatClass SeatClass { get; set; }

        [Required]
        public DateTime FlightDate { get; set; }
    }

}
