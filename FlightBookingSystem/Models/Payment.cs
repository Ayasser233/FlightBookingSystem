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
        [DataType(DataType.Currency)]
        [Range(0, 100000)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Required, MaxLength(50)]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } // e.g., Credit Card, PayPal

        [Required]
        [EnumDataType(typeof(PaymentStatus))]
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
