using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required, MaxLength(50)]
        public string PaymentMethod { get; set; } // e.g., Credit Card, PayPal

        [Required]
        public PaymentStatus Status { get; set; } // Enum: Pending, Completed, Failed

        [MaxLength(100)]
        public string TransactionId { get; set; }
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }
}
