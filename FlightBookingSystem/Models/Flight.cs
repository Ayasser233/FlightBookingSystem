using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.Models
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        [Required, MaxLength(50)]
        [Display(Name = "Flight Number")]
        public string FlightNumber { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Departure Time")]
        public DateTime DepartureTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }

        [Required, MaxLength(100)]
        public string DepartureAirport { get; set; }

        [Required, MaxLength(100)]
        public string ArrivalAirport { get; set; }

        [Required]
        [Display(Name = "Total Seats")]
        public int TotalSeats { get; set; }

        [Display(Name = "Available Seats")]
        public int AvailableSeats => TotalSeats - BookedSeats;

        [Required]
        [Display(Name = "Booked Seats")]
        public int BookedSeats { get; set; }

        [Required]
        public decimal BasePrice { get; set; }

        [Required]
        [EnumDataType(typeof(FlightStatus))]
        public FlightStatus Status { get; set; } // Enum: Scheduled, Cancelled, Delayed

        public ICollection<Booking>? Bookings { get; set; }
    }

    public enum FlightStatus
    {
        Scheduled,
        Cancelled,
        Delayed
    }
}
