using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.Models
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        [Required, MaxLength(50)]
        public string FlightNumber { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required, MaxLength(100)]
        public string DepartureAirport { get; set; }

        [Required, MaxLength(100)]
        public string ArrivalAirport { get; set; }

        [Required]
        public int TotalSeats { get; set; }

        [Required]
        public int AvailableSeats { get; set; }

        [Required]
        public decimal BasePrice { get; set; }

        [Required]
        public FlightStatus Status { get; set; } // Enum: Scheduled, Cancelled, Delayed

        public ICollection<Booking> Bookings { get; set; }
    }

    public enum FlightStatus
    {
        Scheduled,
        Cancelled,
        Delayed
    }
}
