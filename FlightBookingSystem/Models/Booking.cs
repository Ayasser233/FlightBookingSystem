using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User Customer { get; set; }

        [Required]
        public int FlightId { get; set; }

        [ForeignKey("FlightId")]
        public Flight Flight { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public BookingStatus Status { get; set; } // Enum: Pending, Confirmed, Cancelled

        public ICollection<Passenger> Passengers { get; set; }

        [Required]
        public decimal TotalCost { get; set; }

        public Payment Payment { get; set; }
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
}
