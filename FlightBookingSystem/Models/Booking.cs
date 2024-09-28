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
        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }

        [Required]
        [EnumDataType(typeof(BookingStatus))]
        public BookingStatus Status { get; set; } // Enum: Pending, Confirmed, Cancelled

        public ICollection<Passenger> Passengers { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0, 100000)]
        [Display(Name = "Total Cost")]
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
