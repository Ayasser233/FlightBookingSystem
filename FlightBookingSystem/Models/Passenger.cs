using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.Models
{
    public class Passenger
    {
        [Key]
        public int PassengerId { get; set; }

        [Required]
        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, MaxLength(50)]
        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }

        [MaxLength(100)]
        public string Nationality { get; set; }

        [StringLength(10)]
        [Display(Name = "Seat Number")]
        public string? SeatNumber { get; set; }

        [Required]
        [EnumDataType(typeof(SeatClass))]
        public SeatClass SeatClass { get; set; } // Enum: Economy, Business, First
    }

    public enum SeatClass
    {
        Economy,
        Business,
        First
    }
}
